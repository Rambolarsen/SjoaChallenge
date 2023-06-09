using SjoaChallenge.Common;
using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public class LeaderboardService
    {
        private readonly HttpClient _httpClient;
        private const string API = "API";
        private const string ApiUri = "api/leaderboard";
        public LeaderboardService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(API);          
        }

        public async Task AddUser(string username) => 
            await _httpClient.PostAsJsonAsync(ApiUri, username);

        public async Task UpdateLeaderboard(string username) =>
            await _httpClient.PutAsJsonAsync(ApiUri, username);

        public async Task<ICollection<LeaderboardEntry>> GetLeaderboard() => 
            await _httpClient.GetFromJsonAsync<ICollection<LeaderboardEntry>>(ApiUri) ?? new List<LeaderboardEntry>();
    }
}
