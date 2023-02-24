using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderManagementApi.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set;}
        public string? CustomerPhone { get; set;}

        public string? CustomerCity { get; set;}
        public string? CustomerCountry { get; set;}
        public string? CustomerState { get; set;}
        public string? CustomerZipCode { get; set; }
        public string? CustomerAddress { get;set; }

        public string? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set;}
        public string? PaymentStatus { get; set; }

    }
}