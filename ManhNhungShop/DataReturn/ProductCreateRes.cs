using ManhNhungShop.Models;

namespace ManhNhungShop.DataReturn
{
    public class ProductCreateRes
    {
        public bool isSuccess { set; get; }
        public string Message { set; get; }
        public int returncode { set; get; }
        public Products data { get; set; }  
    }
}
