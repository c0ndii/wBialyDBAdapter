using wBialyBezdomnyEdition.Database.Relational.Entities;

namespace wBialyDBAdapter.Repository.Relational
{
    public interface IEventRepository
    {
        Task<int> CreateAsync(Event ev);
        Task<Event?> GetAsync(int id);
        Task<IEnumerable<Event>> GetAllAsync();
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
    }
}
