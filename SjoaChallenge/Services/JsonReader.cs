using System.Net.Http.Json;

namespace SjoaChallenge.Services
{
    public interface IJsonReader
    {
        Task<T?> GetJson<T>(string filename);
    }

    public class JsonReader : IJsonReader
    {
        private readonly HttpClient _httpClient;

        public JsonReader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T?> GetJson<T>(string filename) =>
            await _httpClient.GetFromJsonAsync<T>($"json/{filename}.json");
    }
}
