using Blazored.LocalStorage;
using Blazored.SessionStorage;
using SjoaChallenge.Utilities;

namespace SjoaChallenge.Services
{
    public class UserService
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly ILocalStorageService _localStorage;
        private readonly IUsernameGenerator _usernameGenerator;
        private readonly LeaderboardService _leaderboardService;
        private const string Username = "Username";
        private const string Usernames = "Usernames";
        private const string LoggedIn = "LoggedIn";

        public UserService(ISessionStorageService sessionStorage, 
            ILocalStorageService localStorage,
            IUsernameGenerator usernameGenerator,
            LeaderboardService leaderboardService)
        {
            _sessionStorage = sessionStorage;
            _localStorage = localStorage;
            _usernameGenerator = usernameGenerator;
            _leaderboardService = leaderboardService;
        }

        public async Task<string> GetUsername()
        {
            var username = (await _sessionStorage.GetItemAsync<string>(Username)) ?? string.Empty;
            if (string.IsNullOrEmpty(username))
            {
                var usernames = await _localStorage.GetItemAsync<ICollection<string>>(Usernames);
                username = await GenerateUniqueUsername(usernames);

                usernames ??= new List<string>();
                usernames.Add(username);
                await _localStorage.SetItemAsync(Usernames, usernames);
                await _sessionStorage.SetItemAsync(Username, username);
                await _leaderboardService.AddUser(username);
            }

            return username;
        }

        public async Task<bool> IsLoggedIn()
        {
            if(bool.TryParse((await _sessionStorage.GetItemAsStringAsync(LoggedIn)), out var loggedIn))
                return loggedIn;
            
            return false;
        }

        private async Task<string> GenerateUniqueUsername(ICollection<string>? usernames)
        {
            var username = await _usernameGenerator.GenerateUsername();
            if (usernames?.Contains(username) ?? false)
                await GenerateUniqueUsername(usernames);

            return username;
        }
    }
}
