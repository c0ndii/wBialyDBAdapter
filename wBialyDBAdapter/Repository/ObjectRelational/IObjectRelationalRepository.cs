namespace wBialyDBAdapter.Repository.ObjectRelational
{
    public interface IObjectRelationalRepository<T> where T : class
    {
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}
