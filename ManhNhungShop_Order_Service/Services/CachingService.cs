using ManhNhungShop_Order_Service.Models;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Text.Json;

namespace ManhNhungShop_Order_Service.Services
{
    public class CachingService : ICachingServices
    {
        private IDatabase _database;

        public CachingService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379")     ;
            _database = redis.GetDatabase();
            var ft = _database.FT();
            var json = _database.JSON();

        }
        public T GetData<T>(string key)
        {
            var data = _database.StringGet(key);
            if (!string.IsNullOrEmpty(data))
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }

        public async Task<bool> RefreshData(string key)
        {
            TimeSpan newTime = TimeSpan.FromMinutes(3);
            var data = _database.StringGet(key);
            if (!string.IsNullOrEmpty(data))
            {
                return await _database.KeyExpireAsync(key, newTime);
            }
            return false;
        }

        public object RemoveData(string key)
        {
            var _exist = _database.KeyExists(key);
            if (_exist)
            {
                return _database.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T data, DateTimeOffset expireTime)
        {
            var expirationTime = expireTime.DateTime.Subtract(DateTime.Now);
            var isSeted = _database.StringSet(key, JsonSerializer.Serialize(data), expirationTime);
            return isSeted;
        }


        //check update resdis server

        public async Task<bool> UpdateData<T>(ICollection<T> datacahing, ICollection<T> data)
        {
           
            var firstnotsecond = data.Except(datacahing).ToList();
            var secondnotfirst = datacahing.Except(data).ToList();

            if((firstnotsecond.Count + secondnotfirst.Count) == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
