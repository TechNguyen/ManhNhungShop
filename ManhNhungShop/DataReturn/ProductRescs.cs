using ManhNhungShop.Models;

namespace ManhNhungShop.DataReturn
{
    public class ProductRescs
    {
        public bool isSuccess { get; set; }
        public int pageSize { set; get; }
        public int? pageIndex {  set; get; }
        public int pageCount { set; get; }
        public ICollection<Products> data { set; get; } 
        public string? Message { set; get; }

    }
}
