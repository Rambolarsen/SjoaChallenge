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

        public async Task<IDictionary<string, (int score, DateTime updated)>> GetLeaderboard() => 
            await _httpClient.GetFromJsonAsync<IDictionary<string, (int, DateTime)>>(ApiUri) ?? new Dictionary<string, (int, DateTime)>();
    }
}
