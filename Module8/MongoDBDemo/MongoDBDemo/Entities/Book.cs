using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBDemo.Entities
{
    public class Book
    {
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("genre")]
        public string[] Genre { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }
    }
}
