using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient mongoClient;
        private readonly Random random;

        public SellersController(IConfiguration configuration)
        {
            _configuration = configuration;
            mongoClient = new MongoClient(_configuration.GetConnectionString("odmConString"));
            random = new Random();
        }

        [HttpGet]
        public JsonResult Get()
        {
           

            var allSellerList = mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").AsQueryable();

            return new JsonResult(allSellerList);
        }

        [HttpGet("{SellerId}")]
        public JsonResult Get(int SellerId)
        {
            try
            {
                var seller = mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").Find(u => u.SellerId == SellerId).First();

                return new JsonResult(seller);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        public JsonResult Post(Seller seller)
        {
            
            try
            {
                int countAllSeller = mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").AsQueryable().Count();
                seller.SellerId = countAllSeller + 1;

                mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").InsertOne(seller);
                return new JsonResult("Seller data inserted");
            } catch (Exception ex)
            {
                return new JsonResult("Failed to insert\nError:" + ex.Message);
            }
           
        }

        [HttpPut("{SellerId}")]
        public JsonResult Put(int SellerId,Seller seller)
        {
            
            try
            {
                var foundSeller = mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").Find( x => x.SellerId == SellerId ).FirstOrDefault();

                if (foundSeller != null)
                {
                    seller._id = foundSeller._id;
                    mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").ReplaceOneAsync(x => x._id == seller._id, seller);

                    return new JsonResult("Seller data Updated");
                } else
                {
                    return new JsonResult("Seller data not found");
                }
                
                
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to update\nError:" + ex.Message);
            }

        }

        [HttpDelete("{SellerId}")]
        public JsonResult Delete(int SellerId)
        {

            try
            {
                var foundSeller = mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").Find(x => x.SellerId == SellerId).FirstOrDefault();

                if (foundSeller != null)
                {

                    mongoClient.GetDatabase("ODMdb").GetCollection<Seller>("Sellers").DeleteOneAsync(x => x._id == foundSeller._id);

                    return new JsonResult("Seller data deleted");
                }
                else
                {
                    return new JsonResult("Seller data not found");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to delete\nError:" + ex.Message);
            }

        }
    }
}
