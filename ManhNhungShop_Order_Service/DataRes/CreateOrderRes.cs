using ManhNhungShop_Order_Service.Models;

namespace ManhNhungShop_Order_Service.DataRes
{
    public class CreateOrderRes
    {
        public bool isSucces { set; get; }
        public string Message { set; get; }
        public Order data { set;get; }

    }
}
