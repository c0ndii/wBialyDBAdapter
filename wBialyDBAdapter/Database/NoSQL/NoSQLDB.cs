using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wBialyBezdomnyEdition.Config;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;

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

        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Gastro> Gastros => _database.GetCollection<Gastro>("Gastros");
    }

}
