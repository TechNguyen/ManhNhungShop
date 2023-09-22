namespace ManhNhungShop_Product_Service.Models
{
    public class SaleOff
    {
        public List<string> TypeProduct { get; set; }         
        public int SaleOffPercent { get; set; }
        public IDictionary<string, int> Product { get; set; }
    }
}