using ManhNhungShop.DataReturn;
using ManhNhungShop.Models;
using ManhNhungShop_Product_Service.DataReturn;
using ManhNhungShop_Product_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace ManhNhungShop.Interfaces
{
    public interface IProduct
    {
        Task<ProductRescs> GetAllProduct(int page);
        Task<ProductCreateRes> CreateProduct(Products product, FileUpload files);
        Task<ProductCreateRes> GetProductById(int productId);
        Task<ProductDetailRes> GetProductDetail(int productId);
        ProductMainRes SortByMoney(int typesort);
        ICollection<Products> SortProductsByType(string type);
        Task<ProductUpdateRes> UpdateProduct(ProductsDetails products, int productId);
        Task<bool> DeleteSoftProduct(int productId);
        Task<bool> RestoreProduct(int productId);
        Task<bool> DeleteProduct (int productId );

        //Task<List<Products>>SrtProductByTime();

        Task<List<Products>> GetProductByType(string typeProduct);
        Task<bool> SaleProduct(DateTime date, SaleOff saleoff);
        Task<string> UploadImageToFirebase(string FileName);
        Task<string> Uploadfile(FileUpload fileUpload);
        bool ProductExist(int ProductId);
        bool ProductDeleteSoftExits(int productId);


        Task<List<Categories>> getallCategories();
    }
}

