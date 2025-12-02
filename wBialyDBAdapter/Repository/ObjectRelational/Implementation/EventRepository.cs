using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation
{
	public class EventRepository : IObjectRelationalRepository<Event>
	{
		private readonly ORDB _context;

		public EventRepository(ORDB context)
		{
			_context = context;
		}

		public async Task<Event> CreateAsync(Event ev)
		{
			_context.Events.Add(ev);
			await _context.SaveChangesAsync();
			return ev;
		}

		public async Task<Event?> GetAsync(int id)
		{
			var result = await _context.Events
				.Include(e => e.EventTags)
				.FirstOrDefaultAsync(e => e.PostId == id);
			return result;
		}

		public async Task<IEnumerable<Event>> GetAllAsync()
		{
			return await _context.Events
				.Include(e => e.EventTags)
				.ToListAsync();
		}

		public async Task UpdateAsync(int id, Event ev)
		{
			var existing = await _context.Events
				.Include(e => e.EventTags)
				.FirstOrDefaultAsync(e => e.PostId == id);

			if (existing == null) return;

			existing.Title = ev.Title;
			existing.Description = ev.Description;
			existing.AddDate = ev.AddDate;
			existing.Place = ev.Place;
			existing.Author = ev.Author;
			existing.Link = ev.Link;
			existing.EventDate = ev.EventDate;

			existing.EventTags.Clear();
			foreach (var tag in ev.EventTags)
				existing.EventTags.Add(tag);

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var ev = await _context.Events.FindAsync(id);
			if (ev != null)
			{
				_context.Events.Remove(ev);
				await _context.SaveChangesAsync();
			}
		}
	}
}
