using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation
{
    public class TagEventRepository : IObjectRelationalRepository<Tag_Event>
    {
        private readonly ORDB _context;

        public TagEventRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag_Event>> GetAllAsync()
        {
            return await _context.EventTags.ToListAsync();
        }

        public async Task<Tag_Event> GetByIdAsync(int id)
        {
            return await _context.EventTags.FindAsync(id);
        }

        public async Task AddAsync(Tag_Event entity)
        {
            await _context.EventTags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag_Event entity)
        {
            _context.EventTags.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.EventTags.FindAsync(id);
            if (entity != null)
            {
                _context.EventTags.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Tag_Event?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag_Event> CreateAsync(Tag_Event entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, Tag_Event entity)
        {
            throw new NotImplementedException();
        }
    }
}