using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public interface IBenchmarkService
    {
        Task<BenchmarkResponse> RunBenchmarksAsync(
            BenchmarkRequest request,
            CancellationToken cancellationToken = default);
    }
}