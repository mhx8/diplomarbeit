using Domain.WorkItem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Iteration
{
    public interface IIterationManager
    {
        Task<IEnumerable<Domain.Iteration.IterationModel>> GetReleaseIterationsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<FeatureModel>> GetFeaturesByReleaseIterationAsync(Guid iterationId, CancellationToken cancellationToken);
    }
}