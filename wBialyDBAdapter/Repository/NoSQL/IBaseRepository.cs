using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Repository.NoSQL
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetManyAsync(EndpointRequest request);
        Task<T> GetByKeyAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
