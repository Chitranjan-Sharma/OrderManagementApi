using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderManagementApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? BrandName { get; set;}

        public double MRP { get;set;}

        public DateTime? MFG { get; set;}

        public DateTime? EXP { get; set; }

        public string? Description { get; set;}

        public string? ImageUrl { get; set; }
    }
}
