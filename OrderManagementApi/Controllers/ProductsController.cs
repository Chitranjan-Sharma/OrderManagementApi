using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient mongoClient;
        private readonly Random random;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            mongoClient = new MongoClient(_configuration.GetConnectionString("odmConString"));
            random = new Random();
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var allProductList = mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").AsQueryable();

                return new JsonResult(allProductList);
            } 
            catch(Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("{ProductId}")]
        public JsonResult Get(int ProductId)
        {
            try
            {
                var product = mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").Find(p => p.ProductId == ProductId).First();

                return new JsonResult(product);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        public JsonResult Post(Product product)
        {
            try
            {
                int countAllProducts = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").AsQueryable().Count();
                product.ProductId = countAllProducts + 1;

                mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").InsertOne(product);

                return new JsonResult("Product inserted");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{ProductId}")]
        public JsonResult Put(int ProductId, Product product)
        {
            try
            {
                var foundProduct = mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").Find(p => p.ProductId == ProductId).First();
                if (foundProduct != null)
                {
                    product._id = foundProduct._id;
                    mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").ReplaceOneAsync(p => p._id == product._id, product);
                } else
                {
                    return new JsonResult("Product not found");
                }

                return new JsonResult("Product updated");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{ProductId}")]
        public JsonResult Delete(int ProductId)
        {

            try
            {
                var product = mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").Find(x => x.ProductId == ProductId).FirstOrDefault();

                if (product != null)
                {

                    mongoClient.GetDatabase("ODMdb").GetCollection<Product>("Products").DeleteOneAsync(x => x._id == product._id);

                    return new JsonResult("Product data deleted");
                }
                else
                {
                    return new JsonResult("Product data not found");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to delete\nError:" + ex.Message);
            }

        }
    }
}
