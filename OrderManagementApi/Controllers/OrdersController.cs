using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient mongoClient;
        

        public OrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
            mongoClient = new MongoClient(_configuration.GetConnectionString("odmConString"));
            
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var orderList = mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").AsQueryable();

                return new JsonResult(orderList);
            } 
            catch(Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("{OrderId}")]
        public JsonResult Get(int OrderId)
        {
            try
            {
                var order = mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").Find(o => o.OrderId == OrderId).First();

                return new JsonResult(order);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        public JsonResult Post(Order order)
        {
            try
            {
                int countAllOrders = mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").AsQueryable().Count();
                order.OrderId = countAllOrders + 1;

                mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").InsertOne(order);

                return new JsonResult("Order inserted");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{OrderId}")]
        public JsonResult Put(int OrderId, Order order)
        {
            try
            {
                var foundOrder = mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").Find(o => o.OrderId == OrderId).First();
                if (foundOrder != null)
                {
                    order._id = foundOrder._id;
                    mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").ReplaceOneAsync(o => o._id == order._id, order);
                } else
                {
                    return new JsonResult("Order not found");
                }

                return new JsonResult("Order updated");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{OrderId}")]
        public JsonResult Delete(int OrderId)
        {

            try
            {
                var order = mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").Find(x => x.OrderId == OrderId).FirstOrDefault();

                if (order != null)
                {

                    mongoClient.GetDatabase("ODMdb").GetCollection<Order>("Orders").DeleteOneAsync(x => x._id == order._id);

                    return new JsonResult("Order data deleted");
                }
                else
                {
                    return new JsonResult("Order data not found");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to delete\nError:" + ex.Message);
            }

        }
    }
}
