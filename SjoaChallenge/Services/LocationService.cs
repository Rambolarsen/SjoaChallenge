using Blazored.LocalStorage;
using Blazored.SessionStorage;
using SjoaChallenge.Utilities;

namespace SjoaChallenge.Services
{
    public class LocationService
    {
        private readonly ISessionStorageService _sessionStorage;

        private const string Location = "Location";
        
        public LocationService(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;            
        }

        public async Task<string> GetLocation()
        {
            var location = (await _sessionStorage.GetItemAsync<string>(Location)) ?? string.Empty;
            if (string.IsNullOrEmpty(location))
            {
                location = "/";
                await _sessionStorage.SetItemAsync(Location, location);
            }

            return location;
        }
    }
}
