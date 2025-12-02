using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Mapping
{
    public interface IGastroMapper
    {
        UnifiedGastroModel FromNoSql(Database.NoSQL.Entities.Gastro source);
        UnifiedGastroModel FromRelational(Database.Relational.Entities.Gastro source);
        UnifiedGastroModel FromObjectRelational(Database.ObjectRelational.Entities.Gastro source);

        Database.NoSQL.Entities.Gastro ToNoSql(UnifiedGastroModel source);
        Database.Relational.Entities.Gastro ToRelational(UnifiedGastroModel source);
        Database.ObjectRelational.Entities.Gastro ToObjectRelational(UnifiedGastroModel source);
    }
}
