using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wBialyBezdomnyEdition.Database.NoSQL.Entities
{
    public class Tag
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
    }
}
