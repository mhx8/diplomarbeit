using Application.Iteration;
using Application.Release;
using Domain.Iteration;
using Domain.Release;
using Domain.WorkItem;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevOpsController : ControllerBase
    {
        private readonly IIterationManager _iterationManager;
        private readonly IReleaseManager _releaseManager;

        public DevOpsController(IIterationManager iterationManager, IReleaseManager releaseManager)
        {
            _iterationManager = iterationManager;
            _releaseManager = releaseManager;
        }

        [HttpGet("GetReleasesByWorkItemId")]
        public async Task<IEnumerable<ReleaseModel>> GetReleasesByWorkItemId(int workItemId, CancellationToken cancellationToken = default)
        {
            return await _releaseManager.GetReleasesByWorkItemIdAsync(workItemId, cancellationToken);
        }


        [HttpPost("TriggerRelease")]
        public async Task TriggerRelease([FromBody] TriggerReleaseArgs args, CancellationToken cancellationToken = default)
        {
            await _releaseManager.TriggerReleaseAsync(args, cancellationToken);
        }

        [HttpGet("GetReleaseIterations")]
        public async Task<IEnumerable<IterationModel>> GetReleaseIterations(CancellationToken cancellationToken = default)
        {
            return await _iterationManager.GetReleaseIterationsAsync(cancellationToken);
        }

        [HttpGet("GetFeaturesByReleaseIteration")]
        public async Task<IEnumerable<FeatureModel>> GetFeaturesByReleaseIteration(Guid iterationId, CancellationToken cancellationToken = default)
        {
            return await _iterationManager.GetFeaturesByReleaseIterationAsync(iterationId, cancellationToken);
        }
    }
}