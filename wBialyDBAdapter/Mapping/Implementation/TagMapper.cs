using wBialyDBAdapter.Model;
using NoSql = wBialyDBAdapter.Database.NoSQL.Entities;
using Rel = wBialyDBAdapter.Database.Relational.Entities;
using Obj = wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Mapping.Implementation
{
    public class TagMapper : ITagMapper
    {
        public UnifiedTagModel FromNoSql(NoSql.Tag entity) =>
            new UnifiedTagModel { Id = entity.Id, Name = entity.Name };

        public UnifiedTagModel FromRelationalEvent(Rel.Tag_Event entity) =>
            new UnifiedTagModel { Id = entity.TagID.ToString(), Name = entity.Name };

        public UnifiedTagModel FromRelationalGastro(Rel.Tag_Gastro entity) =>
            new UnifiedTagModel { Id = entity.TagID.ToString(), Name = entity.Name };

        public UnifiedTagModel FromObjectRelationalEvent(Obj.Tag_Event entity) =>
            new UnifiedTagModel { Id = entity.TagID.ToString(), Name = entity.Name };

        public UnifiedTagModel FromObjectRelationalGastro(Obj.Tag_Gastro entity) =>
            new UnifiedTagModel { Id = entity.TagID.ToString(), Name = entity.Name };
    }
}
