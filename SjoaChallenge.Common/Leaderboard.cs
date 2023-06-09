using System.Text.Json.Serialization;

namespace SjoaChallenge.Common
{
    public class LeaderboardEntry
    {
        [JsonConstructor]
        public LeaderboardEntry(string username, int score, DateTime updated)
        {
            Username = username;
            Score = score;
            Updated = updated;
        }

        public LeaderboardEntry(string username, int score) 
        { 
            Username = username;
            Score = score;
            Updated = DateTime.Now;
        }

        public LeaderboardEntry(string username)
        {
            Username=username;
            Score = 0;
            Updated = DateTime.Now;
        }

        public string Username { get; set; }
        public int Score { get; set; }
        public DateTime Updated { get; set; }
    }
}
