using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public interface ITagService
    {
        Task<EndpointResponse<IReadOnlyList<UnifiedTagModel>>> GetAllAsync(
            EndpointRequest request,
            CancellationToken cancellationToken = default);
    }
}