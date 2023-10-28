using ManhNhungShop_Order_Service.Models;
using ManhNhungShop_Order_Service.Repository;
using ManhNhungShop_Order_Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace ManhNhungShop_Order_Service.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRep _orderRep;
        private readonly ICachingServices _cahingserver;
        public OrderController(OrderRep orderRep, ICachingServices cachingserver)
        {
            _orderRep = orderRep; 
            _cahingserver = cachingserver;
        }

        //get all order
        [HttpGet("order")]
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
                var data = await _orderRep.GetAllOrder();

                var datacaching = _cahingserver.GetData<ICollection<Order>>("OrderAll");
                if (data != null && data.Count() > 0)
                {
                    if(data != datacaching)
                    {
                        //Remove data redis
                        _cahingserver.RemoveData("OrderAll");
                        // set data cahing for redis
                        var expriration = DateTime.Now.AddMinutes(30);
                        _cahingserver.SetData<ICollection<Order>>("OrderAll", data, expriration);
                    } else
                    {
                        _cahingserver.RefreshData("OrderAll");
                    }
                }
                return Ok(data);

            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //get all order by userId
        [HttpGet("customerId/{customerId}")]
        public async Task<IActionResult> GetOrderByCustomerId(string customerId)
        {
            try
            {
                var data = await _orderRep.GetOrderById(customerId);
                var datacaching = _cahingserver.GetData<Order>($"OrderById={customerId}");
                //caching redis
                return Ok(data);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //get order
        [HttpGet("orderId/{orderId}")]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
        {
            try
            {
                var orderItem = _orderRep.GetOrder(orderId);
                return Ok(orderItem);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //create a order
        [HttpPost("order/create")]
        public async Task<IActionResult> CreateOrderItem([FromBody] Order orderItem)
        {
            try
            {
                await _orderRep.CreateOrderAsync(orderItem);
                return Ok();
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //Update Order Record
        [HttpPut("update/{objectId}")]
        public async Task<IActionResult> UpdateOrderItem(string objectId, [FromBody] Order OrderUpdate)
        {
            try
            {
                await _orderRep.UpdateOrder(objectId, OrderUpdate);
                return Ok();
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //Delete OrderItem
        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                await _orderRep.DeleteOrder(orderId);
                return Ok();
            } catch (Exception ex)
            {
                return NotFound(ex.Message); 
            }
        }
        //count order in day,week,month
        //public async Task<IActionResult> ManagementCount(string customerId)
        //{
        //    try
        //    {
        //        var manaOrder = _orderRep;
        //        return Ok()
        //    } catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}
    }
}
