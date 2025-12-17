using MongoDB.Bson;
using MongoDB.Driver;
using wBialyDBAdapter.Database.NoSQL;
using wBialyDBAdapter.Database.NoSQL.Entities;
using wBialyDBAdapter.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Repository.NoSQL.Implementation
{
    public class TagRepository : IBaseRepository<Database.NoSQL.Entities.Tag>
    {
        private readonly IMongoCollection<Database.NoSQL.Entities.Tag> _collection;

        public TagRepository(NoSQLDB db)
        {
            _collection = db.Tags;
        }

        public async Task<List<Database.NoSQL.Entities.Tag>> GetManyAsync(EndpointRequest request)
        {
            var find = _collection.Find(FilterDefinition<Database.NoSQL.Entities.Tag>.Empty)
                                  .Skip(request.Skip)
                                  .Limit(request.PageSize);

            return await find.ToListAsync();
        }

        public async Task<Database.NoSQL.Entities.Tag> GetByKeyAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Database.NoSQL.Entities.Tag>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Database.NoSQL.Entities.Tag entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, Database.NoSQL.Entities.Tag entity)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Database.NoSQL.Entities.Tag>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Database.NoSQL.Entities.Tag>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<Database.NoSQL.Entities.Tag> GetByNameAsync(string name)
        {
            var filter = Builders<Database.NoSQL.Entities.Tag>.Filter.Eq(t => t.Name, name);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}