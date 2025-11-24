using MongoDB.Driver;
using wBialyBezdomnyEdition.Database.NoSQL;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;

namespace wBialyBezdomnyEdition.Repository.NoSQL
{
    public class OnSiteRepository : BaseRepository<OnSite>, IOnSiteRepository
    {
        public OnSiteRepository(NoSQLDB db) : base(db) { }
    }

    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(NoSQLDB db) : base(db) { }
    }

    public class GastroRepository : BaseRepository<Gastro>, IGastroRepository
    {
        public GastroRepository(NoSQLDB db) : base(db) { }
    }
}
