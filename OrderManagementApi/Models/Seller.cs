using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderManagementApi.Models
{
    public class Seller
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int SellerId { get; set; }

        public string? SellerName { get; set; }

        public string? CompanyName { get; set; }

        public string? CompanyEmail { get; set; }

        public string? Password { get; set; }
    }
}
