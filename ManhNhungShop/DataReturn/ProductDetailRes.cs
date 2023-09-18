using ManhNhungShop.Models;

namespace ManhNhungShop.DataReturn
{
    public class ProductDetailRes
    {
        public int? returnCode { get; set; }
        public bool? isSuccess { get; set; }
        public string? Message { get; set; }
        public ProductsDetails data { get; set; }
    }
}
