using Microsoft.AspNetCore.Mvc;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Services;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly IBenchmarkService _benchmarkService;

        public BenchmarkController(IBenchmarkService benchmarkService)
        {
            _benchmarkService = benchmarkService;
        }

        [HttpPost("run")]
        public async Task<ActionResult<BenchmarkResponse>> RunBenchmarks(
            [FromBody] BenchmarkRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _benchmarkService.RunBenchmarksAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("run-quick")]
        public async Task<ActionResult<BenchmarkResponse>> RunQuickBenchmarks(
            CancellationToken cancellationToken)
        {
            var request = new BenchmarkRequest
            {
                EntityType = "Event",
                RecordCounts = new List<int> { 1, 100, 1000 },
                Iterations = 5
            };

            var response = await _benchmarkService.RunBenchmarksAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("run-full")]
        public async Task<ActionResult<BenchmarkResponse>> RunFullBenchmarks(
            [FromQuery] DatabaseType dbType,
            [FromQuery] int RecordsCount = 1,
            [FromQuery] string entityType = "Event",
            CancellationToken cancellationToken = default)
        {
            var request = new BenchmarkRequest
            {
                DatabaseType = dbType,
                EntityType = entityType,
                RecordCounts = new List<int> { RecordsCount },
                Iterations = 1
            };

            var response = await _benchmarkService.RunBenchmarksAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}