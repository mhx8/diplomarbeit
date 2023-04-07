using Domain.Release;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Release
{
    public interface IReleaseManager
    {
        Task<IEnumerable<ReleaseModel>> GetReleasesByWorkItemIdAsync(int workItemId, CancellationToken cancellationToken);

        Task TriggerReleaseAsync(TriggerReleaseArgs args, CancellationToken cancellationToken);
    }
}