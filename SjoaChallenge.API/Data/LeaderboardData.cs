using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Data
{
    public interface ILeaderboardData
    {
        Task AddUserToLeaderboard(string user);
        Task<IDictionary<string, (int, DateTime)>> GetLeaderboard();

    }
    public class LeaderboardData : ILeaderboardData
    {
        private readonly IDictionary<string, (int Score, DateTime Updated)> _leaderboard = new Dictionary<string, (int Score, DateTime Updated)>();

        public Task AddUserToLeaderboard(string user)
        {
            if (user != null && !_leaderboard.ContainsKey(user))
            {
                _leaderboard.Add(user, (0,DateTime.Now));
            }

            return Task.CompletedTask;
        }

        public Task<IDictionary<string, (int,DateTime)>> GetLeaderboard() =>
            Task.FromResult(_leaderboard);
    }
}
