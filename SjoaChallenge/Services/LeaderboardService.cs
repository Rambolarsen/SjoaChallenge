using Blazored.LocalStorage;

namespace SjoaChallenge.Services
{
    public class LeaderboardService
    {
        private readonly ILocalStorageService _localStorage;

        private const string Leaderboard = "Leaderboard";
        private IDictionary<string, int> _leaderboard = new Dictionary<string,int>();

        public LeaderboardService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;            
        }

        public async Task AddUser(string username) 
        {
            _leaderboard = await _localStorage.GetItemAsync<IDictionary<string, int>>(Leaderboard) ?? new Dictionary<string, int>();
            if (!_leaderboard.ContainsKey(username) ) 
            {
                _leaderboard.Add(username, 0);
                await _localStorage.SetItemAsync<IDictionary<string, int>>(Leaderboard, _leaderboard);
            }
        }
    }
}
