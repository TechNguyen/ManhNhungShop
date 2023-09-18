using ManhNhungShop_Order_Service.Models;

namespace ManhNhungShop_Order_Service.DataRes
{
    public class GetOrderRescs
    {
        public int rtcode { set; get; }
        public string Message { set; get ; }
        public List<Order> data { set; get; }
        public bool isSuccess { set; get; }

    }
}
