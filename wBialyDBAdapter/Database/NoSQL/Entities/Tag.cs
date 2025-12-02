using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wBialyDBAdapter.Database.NoSQL.Entities
{
    public class Tag
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
    }
}
