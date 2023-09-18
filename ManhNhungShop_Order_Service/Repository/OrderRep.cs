using Amazon.SecurityToken.Model;
using ManhNhungShop_Order_Service.DataRes;
using ManhNhungShop_Order_Service.Models;
using ManhNhungShop_Order_Service.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManhNhungShop_Order_Service.Repository
{
    public class OrderRep
    {
        private readonly IMongoCollection<Order> _orders;

        private readonly ICachingServices _cachingServices;
        public OrderRep(IOptions<OrderDatabaseSetting> orderdatabasesetting, ICachingServices cachingServices)
        {
            var mongoClient = new MongoClient(orderdatabasesetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(orderdatabasesetting.Value.Databasename);
            _orders = mongoDatabase.GetCollection<Order>(orderdatabasesetting.Value.OrderCollectionName);
            _cachingServices = cachingServices;

        }

        //get all order
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> ordersList = new List<Order>();
            return await _orders.Find(_ => true).ToListAsync();
        }
        // get order
        public async Task<Order> GetOrder(string objectId)
        {
            return _orders.Find(od => od.Id == objectId).FirstOrDefault();
        }
        // get order by customerId
        public async Task<GetOrderRescs> GetOrderById(string customerId)
        {

            GetOrderRescs getorderRes = new GetOrderRescs();
            //check cahing data
            var cachingdata = _cachingServices.GetData<ICollection<Order>>($"orderby={customerId}");

            if (cachingdata != null && cachingdata.Count() > 0)
            {
                var orderList = await _orders.Find(x => x.customerId == customerId).ToListAsync();
                if (await _cachingServices.UpdateData(cachingdata, orderList))
                {
                    _cachingServices.RefreshData($"orderby={customerId}");
                }
                else
                {
                    //set data redis again
                    var expriseTime = DateTime.Now.AddMinutes(30);
                    _cachingServices.SetData<List<Order>>($"orderby={customerId}", orderList, expriseTime);
                }
            }
            else
            {
                var orderList = await _orders.Find(x => x.customerId == customerId).ToListAsync();
                var expriationTime = DateTime.Now.AddMinutes(30);
                // caching server
                if (orderList.Count > 0)
                {
                    getorderRes.rtcode = 200;
                    getorderRes.isSuccess = true;
                    getorderRes.Message = "Get List Order successfully";
                    getorderRes.data = orderList;
                    _cachingServices.SetData<ICollection<Order>>($"orderby={customerId}", orderList, expriationTime);
                }
                else
                {
                    getorderRes.rtcode = 401;
                    getorderRes.isSuccess = false;
                    getorderRes.Message = "Not Found Data";
                }
            }
            return getorderRes;
        }
        // create one order items
        public async Task<Order> CreateOrderAsync(Order ordernew)
        {
            await _orders.InsertOneAsync(ordernew);
            return ordernew;
        }
        // delete one order item 
        public Task DeleteOrder(string orderId)
        {
            return _orders.DeleteOneAsync(r => r.Id == orderId);
        }
        public async Task GetOrderByTime(string objectId)
        {
            _orders.DeleteOneAsync(p => p.Id == objectId);
        }
        //update order
        public Task UpdateOrder(string orderIdUpdate, Order orderupdate)
        {
             return _orders.ReplaceOneAsync(p => p.Id == orderupdate.Id, orderupdate);
        }
    }
}
