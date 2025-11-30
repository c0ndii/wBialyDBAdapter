using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational
{
    public interface IEventRepository
    {
        Task<Event> CreateAsync(Event ev);
        Task<Event?> GetAsync(int id);
        Task<IEnumerable<Event>> GetAllAsync();
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
    }
}
