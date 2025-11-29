using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using wBialyBezdomnyEdition.Database.NoSQL;

namespace wBialyBezdomnyEdition.Repository.NoSQL
{
    public class BaseRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseRepository(NoSQLDB db)
        {
            _collection = typeof(T).Name switch
            {
                "OnSite" => db.OnSite as IMongoCollection<T>,
                "Event" => db.Events as IMongoCollection<T>,
                "Gastro" => db.Gastros as IMongoCollection<T>,
                _ => throw new InvalidOperationException($"Unknown collection: {typeof(T).Name}")
            };
        }

        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
                filter = x => true;

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetByKeyAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
