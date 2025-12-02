namespace wBialyDBAdapter.Repository.Relational
{
    public interface IRelationalRepository<T>
    {
        Task<int> CreateAsync(T entity);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
