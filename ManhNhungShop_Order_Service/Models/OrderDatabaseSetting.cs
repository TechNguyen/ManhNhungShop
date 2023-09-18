namespace ManhNhungShop_Order_Service.Models
{
    public class OrderDatabaseSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string Databasename { get; set; } = null!;
        public string OrderCollectionName { get; set; } = null!;
    }
}
