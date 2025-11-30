using Microsoft.EntityFrameworkCore;
using wBialyBezdomnyEdition.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation
{
    public class GastroRepository : IGastroRepository
    {
        private readonly ORDB _context;

        public GastroRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<Gastro> CreateAsync(Gastro g)
        {
            _context.Gastros.Add(g);
            await _context.SaveChangesAsync();
            return g;
        }

        public async Task<Gastro?> GetAsync(int id)
        {
            return await _context.Gastros
                .Include(g => g.GastroTags)
                .FirstOrDefaultAsync(g => g.PostId == id);
        }

        public async Task<IEnumerable<Gastro>> GetAllAsync()
        {
            return await _context.Gastros
                .Include(g => g.GastroTags)
                .ToListAsync();
        }

        public async Task UpdateAsync(Gastro g)
        {
            var existing = await _context.Gastros
                .Include(g => g.GastroTags)
                .FirstOrDefaultAsync(x => x.PostId == g.PostId);

            if (existing == null) return;

            existing.Title = g.Title;
            existing.Description = g.Description;
            existing.AddDate = g.AddDate;
            existing.Place = g.Place;
            existing.Author = g.Author;
            existing.Link = g.Link;
            existing.Day = g.Day;

            // Many-to-Many tag update
            existing.GastroTags.Clear();
            foreach (var tag in g.GastroTags)
            {
                existing.GastroTags.Add(tag);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var g = await _context.Gastros.FindAsync(id);
            if (g != null)
            {
                _context.Gastros.Remove(g);
                await _context.SaveChangesAsync();
            }
        }
    }
}
