using System.Net.Http.Json;

namespace SjoaChallenge.Utilities
{
    public class UsernameGenerator
    {
        private ICollection<string>? _actions;
        private ICollection<string>? _animals;
        private ICollection<string>? _foods;
        private readonly Random _random = new Random();
        private readonly HttpClient _httpClient;

        public UsernameGenerator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<ICollection<string>?> GetJson(string filename) => 
            await _httpClient.GetFromJsonAsync<ICollection<string>>($"json/{filename}.json");

        private string RemoveSpaceAndReplaceNextLetterWithCapital(string input)
        {
            string result = "";
            bool capitalizeNext = false;

            foreach (char c in input)
            {
                if (c == ' ')
                {
                    capitalizeNext = true;
                }
                else
                {
                    result += capitalizeNext ? char.ToUpper(c) : c;
                    capitalizeNext = false;
                }
            }

            return result;
        }

        public async Task<string> GenerateUsername()
        {
            _actions ??= await GetJson("actions");
            _animals ??= await GetJson("animals");
            _foods ??= await GetJson("food");
            var username = GetRawUsername();

            username = RemoveSpaceAndReplaceNextLetterWithCapital(username);
            char[] letters = username.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            return new string(letters);
        }

        private string GetRawUsername()
        {
            var animal = GetRandomEntryFromList(_animals);
            var action = GetRandomEntryFromList(_actions);
            var food = GetRandomEntryFromList(_foods);
            return $"{animal} {action} {food}";
        }

        private string GetRandomEntryFromList(ICollection<string>? collection)
        {
            var animalIndex = _random.Next(collection?.Count ?? 0);
            return collection?.Skip(animalIndex).Take(1).First() ?? string.Empty;
        }
    }
}
