using System.Net.Http.Json;
using System.Text;

namespace SjoaChallenge.Services
{
    public class CommandService
    {
        private readonly HttpClient _httpClient;
        private ICollection<string>? _supportedCommands;

        public CommandService(HttpClient httpClient)
        {
            _httpClient = httpClient;           
            Init();
        }

        private async void Init()
        {
            _supportedCommands = await GetJson("supportedCommands");
        }

        public string HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) {  return "<p>No command given.</p>"; }

            var strings = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (!_supportedCommands!.Contains(strings[0])) { 
                return "<p> Unsupported command. Write 'help' to see supported commands.</p>"; 
            }

            if (strings[0] == "help")
            {
                return ListSupportedCommands();
            }
            //TODO:
            return string.Empty;
        }

        private string ListSupportedCommands()
        {
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine("<p>Supported Commands:</p>");
            stringbuilder.AppendLine("<ul style='list-style-type: none;'>");
            foreach (var supportedCommand in _supportedCommands!)
            {
                stringbuilder.AppendLine($"<li>{supportedCommand}</li>");
            }
            stringbuilder.AppendLine("</ul>");

            return stringbuilder.ToString();
        }

        private async Task<ICollection<string>?> GetJson(string filename) =>
            await _httpClient.GetFromJsonAsync<ICollection<string>>($"json/{filename}.json");
    }
}
