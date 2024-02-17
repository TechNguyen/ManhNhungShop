using ManhNhungShop_Product_Service.DataContext;
using ManhNhungShop_Product_Service.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManhNhungShop_Product_Service.Services.ProductService
{
    public class ProductService : IProductService<Product>
    {
        private readonly MongoDbContext _mongodbContext;
        private readonly IMongoCollection<Product> _products;
        public ProductService(MongoDbContext mongoDbContext) {
            _mongodbContext = mongoDbContext;
            _products = _mongodbContext.GetCollection<Product>("products");
        }
        public async Task<Product> CreateElement(Product element)
        {
            try
            {
                await _products.InsertOneAsync(element);
                return element;
            }
            catch (Exception ex)
            {
                return null;
            }
        } 

        public async Task<DeleteResult> DeleteById(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var rs = await _products.DeleteOneAsync(filter);
            return rs;
        }

        public async Task<IEnumerable<Product>> GetAllSync(int pageSize = 20, int pageIndex = 1)
        {
            var listrs = await _products.Find(x => true).ToListAsync();
            if(pageIndex <= 0)
            {
                return listrs.Skip(pageSize * 1).Take(pageSize);
            } else
            {
                return listrs.Skip(pageSize * pageIndex).Take(pageSize);
            }
        }

        public async Task<Product> GetById(string id)
        {
            return await _products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> Update(string id, Product element)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var update = Builders<Product>.Update.Set(x => x, element);
            var res = await _products.UpdateOneAsync(filter, update);
            if(res.ModifiedCount > 0)
            {
                return await _products.Find(x => x.Id == id).FirstOrDefaultAsync();
            } else
            {
                return null;
            }
        }
    }
}
