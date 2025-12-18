using wBialyDBAdapter.Model;
using NoSql = wBialyDBAdapter.Database.NoSQL.Entities;
using Rel = wBialyDBAdapter.Database.Relational.Entities;
using Obj = wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Mapping
{
    public interface ITagMapper
    {
        UnifiedTagModel FromNoSql(NoSql.Tag entity);

        UnifiedTagModel FromRelational(Rel.Tag entity);
        UnifiedTagModel FromRelationalEvent(Rel.Tag_Event entity);
        UnifiedTagModel FromRelationalGastro(Rel.Tag_Gastro entity);

        UnifiedTagModel FromObjectRelationalEvent(Obj.Tag_Event entity);
        UnifiedTagModel FromObjectRelationalGastro(Obj.Tag_Gastro entity);
    }
}