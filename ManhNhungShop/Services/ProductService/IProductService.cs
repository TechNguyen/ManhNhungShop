using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManhNhungShop_Product_Service.Services.ProductService
{
    public interface IProductService<T> where T : class
    {
        Task<T?> CreateElement(T element);

        Task<IEnumerable<T>> GetAllSync(int pageSize = 20, int pageIndex = 1);

        Task<T> GetById(string id);

        Task<T> Update(string id, T element);

        Task<DeleteResult> DeleteById(string id);
    }
}
