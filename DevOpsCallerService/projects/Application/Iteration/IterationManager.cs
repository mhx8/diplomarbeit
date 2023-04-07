using Domain.Iteration;
using Domain.WorkItem;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Iteration
{
    public class IterationManager : IIterationManager
    {
        private const string ReleasePrefix = "Release_";
        private static readonly Guid ProjectId = new("c8682df7-11d7-4f7d-8b3c-6d0f62150e16");

        private readonly WorkHttpClient workHttpClient;
        private readonly WorkItemTrackingHttpClient workItemTrackingHttpClient;
        private readonly TeamContext context = new(ProjectId);

        public IterationManager(VssConnection connection)
        {
            workHttpClient = connection.GetClient<WorkHttpClient>();
            workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();
        }

        public async Task<IEnumerable<IterationModel>> GetReleaseIterationsAsync(CancellationToken cancellationToken)
        {
            List<TeamSettingsIteration> iterations = await workHttpClient.GetTeamIterationsAsync(context, cancellationToken: cancellationToken);

            List<IterationModel> releaseIterations =
                iterations.Where(_ => _.Name.StartsWith(ReleasePrefix))?.ToList()
                .ConvertAll(MapToIterationModel);

            return releaseIterations ?? Enumerable.Empty<IterationModel>();
        }

        private static IterationModel MapToIterationModel(TeamSettingsIteration teamSettingsIteration)
        {
            IterationModel iterationModel = new();
            iterationModel.Name = teamSettingsIteration.Name;
            iterationModel.Url = teamSettingsIteration.Url;
            iterationModel.Id = teamSettingsIteration.Id;
            iterationModel.Path = teamSettingsIteration.Path;
            return iterationModel;
        }

        public async Task<IEnumerable<FeatureModel>> GetFeaturesByReleaseIterationAsync(Guid iterationId, CancellationToken cancellationToken)
        {
            IterationWorkItems iterationItems = await workHttpClient.GetIterationWorkItemsAsync(context, iterationId, cancellationToken);
            List<DeploymentTaskModel> deploymentTasks = new();
            List<FeatureModel> features = new();

            foreach (WorkItemLink workItemLink in iterationItems?.WorkItemRelations)
            {
                DeploymentTaskModel workItem = new();
                workItem.Id = workItemLink.Target?.Id ?? 0;

                if (workItem.Id != 0)
                {
                    WorkItem workItemDetail = await workItemTrackingHttpClient.GetWorkItemAsync(
                        ProjectId, workItem.Id, expand: WorkItemExpand.Relations, cancellationToken: cancellationToken);

                    workItem.Name = workItemDetail.Fields["System.Title"] as string;
                    workItem.ParentId = Convert.ToInt32(workItemDetail.Fields["System.Parent"]);
                }

                deploymentTasks.Add(workItem);
            }

            IEnumerable<IGrouping<int, DeploymentTaskModel>> groupedByParent = deploymentTasks.GroupBy(_ => _.ParentId);
            foreach (IGrouping<int, DeploymentTaskModel> group in groupedByParent)
            {
                FeatureModel feature = new();
                feature.Id = group.Key;
                feature.Name = (await workItemTrackingHttpClient.GetWorkItemAsync(
                        ProjectId, feature.Id, cancellationToken: cancellationToken))?.Fields["System.Title"] as string;
                feature.Tasks = group.ToList();
                features.Add(feature);
            }

            return features;
        }
    }
}
