using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Mapping
{
    public interface IEventMapper
    {
        UnifiedEventModel FromNoSql(Database.NoSQL.Entities.Event source);
        UnifiedEventModel FromRelational(Database.Relational.Entities.Event source);
        UnifiedEventModel FromObjectRelational(Database.ObjectRelational.Entities.Event source);

        Database.NoSQL.Entities.Event ToNoSql(UnifiedEventModel source);
        Database.Relational.Entities.Event ToRelational(UnifiedEventModel source);
        Database.ObjectRelational.Entities.Event ToObjectRelational(UnifiedEventModel source);
    }
}
