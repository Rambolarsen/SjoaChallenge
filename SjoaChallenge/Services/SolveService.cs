using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public class SolveService
    {
        private const string API = "API";
        private const string ApiUri = "api/solve";
        private readonly HttpClient _httpClient;

        public SolveService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(API);
        }

        public async Task<bool> TrySolve(string currentTask, string phrase) => 
            await _httpClient.GetFromJsonAsync<bool>($"{ApiUri}?currenttask={currentTask}&phrase={phrase}");
    }
}
