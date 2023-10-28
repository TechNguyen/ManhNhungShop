using ManhNhungShop_Product_Service.Models;

namespace ManhNhungShop_Product_Service.DataReturn
{
    public class ProceduCateRes : ProceduRespon
    {
        public ICollection<Categories> listCate { set; get; } 
    }
}
