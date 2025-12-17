using wBialyDBAdapter.Model;
using System.Collections.Generic;
using System.Linq;

namespace wBialyDBAdapter.Mapping.Implementation
{
    public class GastroMapper : IGastroMapper
    {
        public UnifiedGastroModel FromNoSql(Database.NoSQL.Entities.Gastro src)
            => new UnifiedGastroModel
            {
                Id = src.Id,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                Tags = src.Tags?.Select(t => new UnifiedTagModel
                {
                    Id = t.Id.ToString(),
                    Name = t.Name
                }).ToList() ?? new List<UnifiedTagModel>()
            };

        public UnifiedGastroModel FromRelational(Database.Relational.Entities.Gastro src)
            => new UnifiedGastroModel
            {
                Id = src.PostId.ToString(),
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                Tags = src.GastroTags?.Select(t => new UnifiedTagModel
                {
                    Id = t.TagID.ToString(),
                    Name = t.Name
                }).ToList() ?? new List<UnifiedTagModel>()
            };

        public UnifiedGastroModel FromObjectRelational(Database.ObjectRelational.Entities.Gastro src)
            => new UnifiedGastroModel
            {
                Id = src.PostId.ToString(),
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                Tags = src.GastroTags?.Select(t => new UnifiedTagModel
                {
                    Id = t.TagID.ToString(),
                    Name = t.Name
                }).ToList() ?? new List<UnifiedTagModel>()
            };


        // ----------- REVERSE -----------

        public Database.NoSQL.Entities.Gastro ToNoSql(UnifiedGastroModel src)
            => new Database.NoSQL.Entities.Gastro
            {
                Id = src.Id,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                Tags = src.Tags?.Select(t => new Database.NoSQL.Entities.Tag { Id = t.Id, Name = t.Name }).ToList() ?? new List<Database.NoSQL.Entities.Tag>()
            };

        public Database.Relational.Entities.Gastro ToRelational(UnifiedGastroModel src)
            => new Database.Relational.Entities.Gastro
            {
                PostId = int.TryParse(src.Id, out var id) ? id : 0,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                GastroTags = src.Tags?.Select(t => new Database.Relational.Entities.Tag_Gastro { Name = t.Name }).ToList() ?? new List<Database.Relational.Entities.Tag_Gastro>()
            };

        public Database.ObjectRelational.Entities.Gastro ToObjectRelational(UnifiedGastroModel src)
            => new Database.ObjectRelational.Entities.Gastro
            {
                PostId = int.TryParse(src.Id, out var id) ? id : 0,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                Day = src.Day,
                GastroTags = src.Tags?.Select(t => new Database.ObjectRelational.Entities.Tag_Gastro { Name = t.Name }).ToList() ?? new List<Database.ObjectRelational.Entities.Tag_Gastro>()
            };
    }
}
