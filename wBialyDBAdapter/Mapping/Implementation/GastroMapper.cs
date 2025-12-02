using wBialyDBAdapter.Model;

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
                Tags = src.Tags.Select(t => t.Name).ToList()
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
                Tags = src.GastroTags.Select(t => t.Name).ToList()
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
                Tags = src.GastroTags.Select(t => t.Name).ToList()
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
                Tags = src.Tags.Select(x => new Database.NoSQL.Entities.Tag { Name = x }).ToList()
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
                GastroTags = src.Tags.Select(x => new Database.Relational.Entities.Tag_Gastro { Name = x }).ToList()
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
                GastroTags = src.Tags.Select(x => new Database.ObjectRelational.Entities.Tag_Gastro { Name = x }).ToList()
            };
    }

}
