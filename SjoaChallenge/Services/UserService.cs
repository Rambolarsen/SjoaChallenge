using Blazored.LocalStorage;
using SjoaChallenge.Utilities;
using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public class UserService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IUsernameGenerator _usernameGenerator;
        private readonly LeaderboardService _leaderboardService;
        private readonly HttpClient _httpClient;
        private const string Username = "Username";
        private const string LoggedIn = "LoggedIn";
        private const string API = "API";
        private const string ApiUri = "api/users";

        public UserService(ILocalStorageService localStorage,
            IUsernameGenerator usernameGenerator,
            LeaderboardService leaderboardService,
            IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _usernameGenerator = usernameGenerator;
            _leaderboardService = leaderboardService;
            _httpClient = httpClientFactory.CreateClient(API);
        }

        public async Task<string> GetUsername()
        {
            var username = (await _localStorage.GetItemAsync<string>(Username)) ?? string.Empty;
            if (string.IsNullOrEmpty(username))
            {
                var usernames = await _httpClient.GetFromJsonAsync<ICollection<string>>(ApiUri);
                username = await GenerateUniqueUsername(usernames);

                await _httpClient.PostAsJsonAsync(ApiUri, username);
                await _localStorage.SetItemAsync(Username, username);
                await _leaderboardService.AddUser(username);
            }

            return username;
        }

        public async Task<bool> IsLoggedIn() => await _localStorage.GetItemAsync<bool>(LoggedIn);

        public async Task LogIn() => await _localStorage.SetItemAsync(LoggedIn, true);

        private async Task<string> GenerateUniqueUsername(ICollection<string>? usernames)
        {
            var username = await _usernameGenerator.GenerateUsername();
            if (usernames?.Contains(username) ?? false)
                await GenerateUniqueUsername(usernames);

            return username;
        }
    }
}
