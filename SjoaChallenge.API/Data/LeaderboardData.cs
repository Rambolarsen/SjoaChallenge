using SjoaChallenge.Common;
using SjoaChallenge.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Data
{
    public interface ILeaderboardData
    {
        Task AddUserToLeaderboard(string user);
        Task<ICollection<LeaderboardEntry>> GetLeaderboard();
        Task UpdateLeaderboard(string user);

    }
    public class LeaderboardData : ILeaderboardData
    {
        private readonly ICollection<LeaderboardEntry> _leaderboard = new List<LeaderboardEntry>();

        public Task AddUserToLeaderboard(string user)
        {
            if (user != null && _leaderboard.All(x => !x.Username.EqualsIgnoreCase(user)))
            {
                _leaderboard.Add(new LeaderboardEntry(user));
            }

            return Task.CompletedTask;
        }

        public Task<ICollection<LeaderboardEntry>> GetLeaderboard() =>
            Task.FromResult(_leaderboard);

        public Task UpdateLeaderboard(string user)
        {
            var userRecord = _leaderboard.FirstOrDefault(x => x.Username.EqualsIgnoreCase(user));
            if (userRecord == default)
                _leaderboard.Add(new LeaderboardEntry(user, 1));
            else
                userRecord.Score++;
            return Task.CompletedTask;
        }
    }
}
