using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManhNhungShop_Order_Service.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; } = String.Empty;
        public string customerName { set; get; }
        public string customerId { set; get; }
        public string productName { set; get; }
        public int productId { set; get; }
        public DateTime createAt { set; get; }
        public DateTime modify { set; get; }
    }   
}
