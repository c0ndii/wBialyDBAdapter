using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public interface IQueryService<T> where T : class
    {
        Task<EndpointResponse<T?>> GetByKeyAsync(
            BaseRequest request,
            string id,
            CancellationToken cancellationToken = default);

        Task<EndpointResponse<IReadOnlyList<T>>> GetManyAsync(
            EndpointRequest request,
            CancellationToken cancellationToken = default);

        Task<EndpointResponse<IReadOnlyList<T>>> AddAsync(
            PostRequest<T> request,
            CancellationToken cancellationToken = default);

        Task<EndpointResponse<IReadOnlyList<T>>> UpdateAsync(
            PostRequest<T> request,
            string id,
            CancellationToken cancellationToken = default);

        Task<EndpointResponse<bool>> DeleteAsync(
            BaseRequest request,
            string id,
            CancellationToken cancellationToken = default);
    }
}
