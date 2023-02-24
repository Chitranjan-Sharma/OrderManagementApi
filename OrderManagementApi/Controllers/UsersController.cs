using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient mongoClient;
        private readonly Random random;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            mongoClient = new MongoClient(_configuration.GetConnectionString("odmConString"));
            random = new Random();
        }

        [HttpGet]
        public JsonResult Get()
        {
           

            var allUserList = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").AsQueryable();

            return new JsonResult(allUserList);
        }

        [HttpGet("{UserId}")]
        public JsonResult Get(int UserId)
        {
            try
            {
                var user = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").Find(u => u.UserId == UserId).First();

                return new JsonResult(user);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            
            try
            {
                int countAllUser = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").AsQueryable().Count();
                user.UserId = countAllUser + 1;

                mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").InsertOne(user);
                return new JsonResult("User data inserted");
            } catch (Exception ex)
            {
                return new JsonResult("Failed to insert\nError:" + ex.Message);
            }
           
        }

        [HttpPut("{UserId}")]
        public JsonResult Put(int UserId,User user)
        {
            
            try
            {
                var foundUser = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").Find( x => x.UserId == UserId ).FirstOrDefault();

                if (foundUser != null)
                {
                    user._id = foundUser._id;
                    mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").ReplaceOneAsync(x => x._id == user._id, user);

                    return new JsonResult("User data Updated");
                } else
                {
                    return new JsonResult("User data not found");
                }
                
                
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to update\nError:" + ex.Message);
            }

        }

        [HttpDelete("{UserId}")]
        public JsonResult Delete(int UserId)
        {

            try
            {
                var foundUser = mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").Find(x => x.UserId == UserId).FirstOrDefault();

                if (foundUser != null)
                {

                    mongoClient.GetDatabase("ODMdb").GetCollection<User>("Users").DeleteOneAsync(x => x._id == foundUser._id);

                    return new JsonResult("User data deleted");
                }
                else
                {
                    return new JsonResult("User data not found");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Failed to delete\nError:" + ex.Message);
            }

        }
    }
}
