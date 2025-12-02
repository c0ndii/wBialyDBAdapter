using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wBialyDBAdapter.Config;
using wBialyDBAdapter.Database.NoSQL.Entities;

namespace wBialyDBAdapter.Database.NoSQL
{
    public class NoSQLDB
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
