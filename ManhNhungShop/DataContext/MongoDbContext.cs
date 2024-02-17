using ManhNhungShop.Models;
using ManhNhungShop_Product_Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ManhNhungShop_Product_Service.DataContext
{
    public class MongoDbContext
    {
        private readonly IOptions<MongoDbSetting> _MongoDbSetting;
        private readonly IMongoDatabase _mongoDatabase;
        public MongoDbContext(IOptions<MongoDbSetting> MongoDbSetting)
        {
            _MongoDbSetting = MongoDbSetting;
            MongoClient mongoClient = new MongoClient(_MongoDbSetting.Value.ConnectionMongo);
            _mongoDatabase = mongoClient.GetDatabase(_MongoDbSetting.Value.DatabaseMongo);
        }

        public IMongoCollection<T> GetCollection<T>(string namecollect)
        {
            return _mongoDatabase.GetCollection<T>(namecollect);
        }
    }
}
