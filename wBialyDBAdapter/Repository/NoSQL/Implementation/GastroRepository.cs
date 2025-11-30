using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using wBialyBezdomnyEdition.Database.NoSQL;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Repository.NoSQL.Implementation
{
    public class GastroRepository : IBaseRepository<Gastro>
    {
        private readonly IMongoCollection<Gastro> _collection;

        public GastroRepository(NoSQLDB db)
        {
            _collection = db.Gastros;
        }

        public async Task<List<Gastro>> GetManyAsync(EndpointRequest request)
        {
            var find = _collection.Find(FilterDefinition<Gastro>.Empty)
                      .Skip(request.Skip)
                      .Limit(request.PageSize);

            return await find.ToListAsync();
        }

        public async Task<Gastro> GetByKeyAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Gastro>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Gastro entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, Gastro entity)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Gastro>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<Gastro>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<List<Gastro>> GetByDayAsync(DateTime day)
        {
            var filter = Builders<Gastro>.Filter.Eq(g => g.Day, day.Date);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Gastro>> GetByTagAsync(string tagName)
        {
            var filter = Builders<Gastro>.Filter.ElemMatch(g => g.Tags, t => t.Name == tagName);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
