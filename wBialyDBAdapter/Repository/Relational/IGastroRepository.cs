using wBialyBezdomnyEdition.Database.Relational.Entities;

namespace wBialyDBAdapter.Repository.Relational
{
    public interface IGastroRepository
    {
        Task<int> CreateAsync(Gastro g);
        Task<Gastro?> GetAsync(int id);
        Task<IEnumerable<Gastro>> GetAllAsync();
        Task UpdateAsync(Gastro g);
        Task DeleteAsync(int id);
    }
}
