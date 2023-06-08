using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SjoaChallenge.API.Data;

namespace SjoaChallenge.API.Functions
{
    public class LeaderboardGet
    {
        private readonly ILeaderboardData _leaderboardData;

        public LeaderboardGet(ILeaderboardData leaderboardData)
        {
            _leaderboardData = leaderboardData;
        }

        [FunctionName("LeaderboardGet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "leaderboard")] HttpRequest req)
        {
            var leaderboard = await _leaderboardData.GetLeaderboard();
            return new OkObjectResult(leaderboard);
        }
    }

    public class LeaderboardPost
    {
        private readonly ILeaderboardData _leaderboardData;

        public LeaderboardPost(ILeaderboardData leaderboardData)
        {
            _leaderboardData = leaderboardData;
        }

        [FunctionName("LeaderboardPost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "leaderboard")] HttpRequest req, ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonSerializer.Deserialize<string>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await _leaderboardData.AddUserToLeaderboard(user);
            return new OkResult();
        }
    }
}
