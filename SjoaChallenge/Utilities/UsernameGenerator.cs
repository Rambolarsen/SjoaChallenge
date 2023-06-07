using SjoaChallenge.Services;
using System.Net.Http.Json;

namespace SjoaChallenge.Utilities
{
    public interface IUsernameGenerator
    {
        Task<string> GenerateUsername();
    }

    public class UsernameGenerator : IUsernameGenerator
    {
        private ICollection<string>? _actions;
        private ICollection<string>? _animals;
        private ICollection<string>? _foods;
        private readonly Random _random = new Random();
        private readonly IJsonReader _jsonReader;

        public UsernameGenerator(IJsonReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

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
            _actions ??= await _jsonReader.GetJson<ICollection<string>>("actions");
            _animals ??= await _jsonReader.GetJson<ICollection<string>>("animals");
            _foods ??= await _jsonReader.GetJson<ICollection<string>>("food");
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
