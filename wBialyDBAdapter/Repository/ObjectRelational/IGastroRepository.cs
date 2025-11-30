using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational
{
    public interface IGastroRepository
    {
        Task<Gastro> CreateAsync(Gastro g);
        Task<Gastro?> GetAsync(int id);
        Task<IEnumerable<Gastro>> GetAllAsync();
        Task UpdateAsync(Gastro g);
        Task DeleteAsync(int id);
    }
}
