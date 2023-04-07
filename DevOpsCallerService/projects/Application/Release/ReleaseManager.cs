using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.WebApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Release;
using AzureRelease = Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release;
using ReleaseArtifact = Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts.Artifact;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;

namespace Application.Release
{
    public class ReleaseManager : IReleaseManager
    {
        private static readonly Guid ProjectId = new("c8682df7-11d7-4f7d-8b3c-6d0f62150e16");
        private const string ProdEnvironment = "PROD";

        private readonly ReleaseHttpClient releaseHttpClient;
        private readonly BuildHttpClient buildHttpClient;

        public ReleaseManager(VssConnection connection)
        {
            releaseHttpClient = connection.GetClient<ReleaseHttpClient>();
            buildHttpClient = connection.GetClient<BuildHttpClient>();
        }

        public async Task<IEnumerable<ReleaseModel>> GetReleasesByWorkItemIdAsync(int workItemId, CancellationToken cancellationToken)
        {
            List<ReleaseModel> result = new();
            List<AzureRelease> allAzureReleases = await releaseHttpClient.GetReleasesAsync(
                ProjectId,
                expand: ReleaseExpands.Environments,
                queryOrder: ReleaseQueryOrder.Descending,
                cancellationToken: cancellationToken);
            if (allAzureReleases == null || allAzureReleases.Count == 0)
            {
                return result;
            }

            IEnumerable<IGrouping<string, AzureRelease>> groupedAzureReleases = allAzureReleases.GroupBy(_ => _.ReleaseDefinitionReference.Name);
            foreach (IGrouping<string, AzureRelease> azureReleaseGroup in groupedAzureReleases)
            {
                foreach (AzureRelease azureRelease in azureReleaseGroup)
                {
                    AzureRelease release = await releaseHttpClient.GetReleaseAsync(ProjectId, releaseId: azureRelease.Id);
                    ReleaseArtifact releaseArtifact = release.Artifacts.FirstOrDefault();

                    if (releaseArtifact != null)
                    {
                        int buildRunId = Convert.ToInt32(releaseArtifact.DefinitionReference["version"].Id);
                        string buildVersion = releaseArtifact.DefinitionReference["version"].Name;

                        List<ResourceRef> buildWorkItemList = await buildHttpClient.GetBuildWorkItemsRefsAsync(ProjectId, buildRunId);
                        if (buildWorkItemList.Any(_ => _.Id == workItemId.ToString()))
                        {
                            result.Add(MapToReleaseModel(release, allAzureReleases));
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public async Task TriggerReleaseAsync(TriggerReleaseArgs args, CancellationToken cancellationToken)
        {
            ReleaseEnvironmentUpdateMetadata metadata = new();
            metadata.Status = EnvironmentStatus.InProgress;
            metadata.Comment = "By API";

            await releaseHttpClient.UpdateReleaseEnvironmentAsync(
                metadata, ProjectId, args.ReleaseId, args.EnvironmentId, cancellationToken: cancellationToken);
        }

        private static ReleaseModel MapToReleaseModel(AzureRelease release, List<AzureRelease> allReleases)
        {
            ReleaseModel releaseModel = new();
            releaseModel.Id = release.Id;
            releaseModel.Name = release.Name;

            ReleaseEnvironment prodEnv = release.Environments.Where(_ => _.Name == ProdEnvironment).FirstOrDefault();
            if (prodEnv != null)
            {
                releaseModel.EnvironmentId = prodEnv.Id;
                releaseModel.Status = MapToReleaseStatus(prodEnv.Status);
                releaseModel.Url = (prodEnv.ReleaseReference.Links?.Links?["web"] as ReferenceLink)?.Href;
            }

            releaseModel.AlreadyHigherVersionDeployedOrTriggered =
                CheckForLaterDeployedOrTriggeredReleases(release, allReleases);

            return releaseModel;
        }

        private static bool CheckForLaterDeployedOrTriggeredReleases(
            AzureRelease release,
            List<AzureRelease> allReleases)
        {
            return allReleases.Where(
                _ => _.ReleaseDefinitionReference.Name == release.ReleaseDefinitionReference.Name
                && _.Environments.FirstOrDefault(env => env.Name == ProdEnvironment)?.Status
                is EnvironmentStatus.Succeeded or EnvironmentStatus.InProgress)
                .OrderByDescending(_ => _.CreatedOn).FirstOrDefault()?.CreatedOn > release.CreatedOn;
        }

        private static Domain.Release.ReleaseStatus MapToReleaseStatus(EnvironmentStatus status)
        {
            return status switch
            {
                EnvironmentStatus.Undefined or
                EnvironmentStatus.NotStarted or
                EnvironmentStatus.Canceled or EnvironmentStatus.Rejected =>
                    Domain.Release.ReleaseStatus.NotDeployed,
                EnvironmentStatus.InProgress =>
                    Domain.Release.ReleaseStatus.WaitingForApproval,
                EnvironmentStatus.Succeeded or
                EnvironmentStatus.PartiallySucceeded => 
                    Domain.Release.ReleaseStatus.Succeeded,
                EnvironmentStatus.Scheduled or
                EnvironmentStatus.Queued => 
                    Domain.Release.ReleaseStatus.InProgress,
                _ => Domain.Release.ReleaseStatus.NotDeployed,
            };
        }
    }
}
