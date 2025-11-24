using System.Linq.Expressions;

namespace wBialyBezdomnyEdition.Repository.NoSQL
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetManyAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByKeyAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
