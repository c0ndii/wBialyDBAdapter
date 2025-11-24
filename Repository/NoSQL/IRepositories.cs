using wBialyBezdomnyEdition.Database.NoSQL.Entities;

namespace wBialyBezdomnyEdition.Repository.NoSQL
{
    public interface IOnSiteRepository : IBaseRepository<OnSite> { }
    public interface IEventRepository : IBaseRepository<Event> { }
    public interface IGastroRepository : IBaseRepository<Gastro> { }    
}
