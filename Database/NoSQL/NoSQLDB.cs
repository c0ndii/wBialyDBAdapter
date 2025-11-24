using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wBialyBezdomnyEdition.Config;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using Tag = wBialyBezdomnyEdition.Database.NoSQL.Entities.Tag;

namespace wBialyBezdomnyEdition.Database.NoSQL
{
    public class NoSQLDB : IDatabase
    {
        private readonly IMongoDatabase _database;

        public NoSQLDB(IOptions<NoSQLDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Gastro> Gastros => _database.GetCollection<Gastro>("Gastros");
        public IMongoCollection<Tag> Tags => _database.GetCollection<Tag>("Tags");
    }

}
