using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Mapping.Implementation
{
    public class EventMapper : IEventMapper
    {
        public UnifiedEventModel FromNoSql(Database.NoSQL.Entities.Event src)
            => new UnifiedEventModel
            {
                Id = src.Id,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                Tags = src.Tags.Select(t => t.Name).ToList()
            };

        public UnifiedEventModel FromRelational(Database.Relational.Entities.Event src)
            => new UnifiedEventModel
            {
                Id = src.PostId.ToString(),
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                Tags = src.EventTags.Select(t => t.Name).ToList()
            };

        public UnifiedEventModel FromObjectRelational(Database.ObjectRelational.Entities.Event src)
            => new UnifiedEventModel
            {
                Id = src.PostId.ToString(),
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                Tags = src.EventTags.Select(t => t.Name).ToList()
            };


        // ----------- REVERSE -----------

        public Database.NoSQL.Entities.Event ToNoSql(UnifiedEventModel src)
            => new Database.NoSQL.Entities.Event
            {
                Id = src.Id,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                Tags = new List<Database.NoSQL.Entities.Tag>()
            };

        public Database.Relational.Entities.Event ToRelational(UnifiedEventModel src)
            => new Database.Relational.Entities.Event
            {
                PostId = int.TryParse(src.Id, out var id) ? id : 0,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                EventTags = new List<Database.Relational.Entities.Tag_Event>()
            };

        public Database.ObjectRelational.Entities.Event ToObjectRelational(UnifiedEventModel src)
            => new Database.ObjectRelational.Entities.Event
            {
                PostId = int.TryParse(src.Id, out var id) ? id : 0,
                Title = src.Title,
                Description = src.Description,
                AddDate = src.AddDate,
                Author = src.Author,
                Place = src.Place,
                Link = src.Link,
                EventDate = src.EventDate,
                EventTags = new List<Database.ObjectRelational.Entities.Tag_Event>()
            };
    }

}
