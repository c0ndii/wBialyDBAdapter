using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation
{
    public class TagGastroRepository : IObjectRelationalRepository<Tag_Gastro>
    {
        private readonly ORDB _context;

        public TagGastroRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag_Gastro>> GetAllAsync()
        {
            return await _context.GastroTags.ToListAsync();
        }

        public async Task<Tag_Gastro> GetByIdAsync(int id)
        {
            return await _context.GastroTags.FindAsync(id);
        }

        public async Task AddAsync(Tag_Gastro entity)
        {
            await _context.GastroTags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag_Gastro entity)
        {
            _context.GastroTags.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.GastroTags.FindAsync(id);
            if (entity != null)
            {
                _context.GastroTags.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Tag_Gastro?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag_Gastro> CreateAsync(Tag_Gastro entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, Tag_Gastro entity)
        {
            throw new NotImplementedException();
        }
    }
}