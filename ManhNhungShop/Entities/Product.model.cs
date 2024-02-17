using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManhNhungShop_Product_Service.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? productName { get; set; }
        public int quanlity { get; set; }
        public long price { get; set; }
        public string? description { get; set; }
        public string? user_manual { get; set; }
        public string? Ingredient { get; set; }
        public string? Preserve { get; set; }
        public int discount { get; set; }
        public ObjectId? brandId { get; set; }
        public string? origin { get; set; }
        public int views { get; set; }
        public int EvaluteCount { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? InputDay_warehouse { get; set; }
        public string? package { get; set; }
        [BsonRepresentation(BsonType.DateTime)]

        public DateTime? createAt { get; set; }
        [BsonRepresentation(BsonType.DateTime)]

        public DateTime? updateAt { get; set; }
        public bool? updated { get; set; }
        public bool? deleted { get; set; }
        [BsonRepresentation(BsonType.DateTime)]

        public DateTime? deleteAt { get; set; }



        public Product()
        {
            updated = false;
            deleted = false;
            createAt = DateTime.Now;
            InputDay_warehouse = DateTime.Now;
        }

    }
}
