using SjoaChallenge.Common;
using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public class SolveService
    {
        private const string API = "API";
        private const string ApiUri = "api/solve";
        private readonly HttpClient _httpClient;
        private readonly UserService _userService;
        private readonly LeaderboardService _leaderboardService;
        private readonly TaskSolverService _taskSolverService;

        public SolveService(IHttpClientFactory httpClientFactory, 
            UserService userService, 
            LeaderboardService leaderboardService,
            TaskSolverService taskSolverService)
        {
            _httpClient = httpClientFactory.CreateClient(API);
            _userService = userService;
            _leaderboardService = leaderboardService;
            _taskSolverService = taskSolverService;
        }

        public async Task<string> Solve(string currentTask, string phrase)
        {
            var username = await _userService.GetUsername();

            var duplicate = await _taskSolverService.IsDuplicate(username, currentTask);
            if (duplicate)
            {
                return "<p> You have already solved this.";
            }

            var solved = await _httpClient.GetFromJsonAsync<bool>($"{ApiUri}?currenttask={currentTask}&phrase={phrase}");
            if (solved)
            {
                await _leaderboardService.UpdateLeaderboard(username);
                await _taskSolverService.SolveTask(new TaskSolver(username, currentTask));
                return "<p>Congratulatons! That was the correct answer!</p>";
            }
            return "<p>Wrong answer! Try again.</p>";
        } 
            
    }
}
