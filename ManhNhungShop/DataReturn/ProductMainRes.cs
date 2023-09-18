using ManhNhungShop.Models;

namespace ManhNhungShop.DataReturn
{
    public class ProductMainRes
    {
        public int returncode { set; get; } 
        public bool isSuccess {  set; get; } 
        public string Message { set; get; }
        public ICollection<Products> data { set; get; }
    }
}
