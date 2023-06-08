using System.Collections.Generic;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Data
{
    public interface IUserData
    {
        Task<string> AddUser(string name);
        Task<ICollection<string>> GetUsers();
    }

    public class UserData : IUserData
    {
        private readonly ICollection<string> _users = new List<string>();

        public Task<string> AddUser(string name)
        {
            if (!_users.Contains(name))
                _users.Add(name);

            return Task.FromResult(name);
        }

        public Task<ICollection<string>> GetUsers()
        {
            return Task.FromResult(_users);
        }
    }
}
