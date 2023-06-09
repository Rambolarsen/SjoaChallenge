using SjoaChallenge.Common;
using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public class TaskSolverService
    {

        private const string API = "API";
        private const string ApiUri = "api/tasksolver";
        private readonly HttpClient _httpClient;

        public TaskSolverService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(API);
        }

        public async Task<bool> IsDuplicate(string username, string currentTask) =>
            await _httpClient.GetFromJsonAsync<bool>($"{ApiUri}?username={username}&currenttask={currentTask}");

        public async Task SolveTask(TaskSolver taskSolver) =>
            await _httpClient.PostAsJsonAsync(ApiUri,taskSolver);
    }
}
