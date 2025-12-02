using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using wBialyDBAdapter.Database.NoSQL;
using wBialyDBAdapter.Database.NoSQL.Entities;
using wBialyDBAdapter.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Repository.NoSQL.Implementation
{
    public class EventRepository : IBaseRepository<Event>
    {
        private readonly IMongoCollection<Event> _collection;

        public EventRepository(NoSQLDB db)
        {
            _collection = db.Events;
        }

        public async Task<List<Event>> GetManyAsync(EndpointRequest request)
        {
            var find = _collection.Find(FilterDefinition<Event>.Empty)
                      .Skip(request.Skip)
                      .Limit(request.PageSize);

            return await find.ToListAsync();
        }

        public async Task<Event> GetByKeyAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Event>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Event entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, Event entity)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Event>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Event>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
