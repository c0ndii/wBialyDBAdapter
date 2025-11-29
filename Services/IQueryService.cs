using Microsoft.AspNetCore.DataProtection.KeyManagement;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services
{
    public interface IQueryService<T> where T : class
    {
        Task<EndpointResponse<T?>> GetByKeyAsync(EndpointRequest request, string id, CancellationToken cancellationToken = default);
        Task<EndpointResponse<IReadOnlyList<T>>> GetManyAsync(EndpointRequest request, CancellationToken cancellationToken = default);

        Task<EndpointResponse<IReadOnlyList<T>>> AddAsync(EndpointRequest request, T data, CancellationToken cancellationToken = default);
        Task<EndpointResponse<IReadOnlyList<T>>> UpdateAsync(EndpointRequest request, string id, T data, CancellationToken cancellationToken = default);

        Task<EndpointResponse<bool>> DeleteAsync(EndpointRequest request, string id, CancellationToken cancellationToken = default);
    }
}
