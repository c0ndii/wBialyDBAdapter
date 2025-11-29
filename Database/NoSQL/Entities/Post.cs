    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    namespace wBialyBezdomnyEdition.Database.NoSQL.Entities
    {
        public abstract class Post
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime AddDate { get; set; }
            public string Place { get; set; }
            public string Author { get; set; }
        }
    }
