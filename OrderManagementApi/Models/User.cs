using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderManagementApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? Password { get; set; }
    }
}
