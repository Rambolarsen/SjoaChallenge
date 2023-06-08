using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SjoaChallenge.API.Data;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Functions
{
    public class UsersGet
    {
        private readonly IUserData userData;

        public UsersGet(IUserData userData)
        {
            this.userData = userData;
        }

        [FunctionName("UsersGet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
        {
            var users = await userData.GetUsers();
            return new OkObjectResult(users);
        }
    }

    public class UsersPost
    {
        private readonly IUserData userData;

        public UsersPost(IUserData userData)
        {
            this.userData = userData;
        }

        [FunctionName("UsersPost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonSerializer.Deserialize<string>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var newUser = await userData.AddUser(user);
            return new OkObjectResult(newUser);
        }
    }

}
