using System.Net.Http.Json;
using System.Text;

namespace SjoaChallenge.Services
{
    public class CommandService
    {
        private readonly IJsonReader _jsonReader;
        private readonly LocationService _locationService;
        private ICollection<string>? _supportedCommands;

        public CommandService(IJsonReader jsonReader, LocationService locationService)
        {
            _jsonReader = jsonReader;
            _locationService = locationService;
            Init();
        }

        private async void Init()
        {
            _supportedCommands = await _jsonReader.GetJson<ICollection<string>>("supportedCommands");
        }

        public async Task<(string html,string directory)> HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) {  return ("<p>No command given.</p>", string.Empty); }

            var strings = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var command = strings[0];
            if (!_supportedCommands!.Contains(strings[0])) { 
                return ("<p> Unsupported command. Write 'help' to see supported commands.</p>", string.Empty); 
            }

            if(string.Compare(command, "ls", true) == 0 || string.Compare(command, "dir", true) == 0)
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return ListDirectory();
            }

            if(string.Compare(command, "cd", true) == 0)
            {
                if (strings.Length != 2) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return await ChangeDirectory(strings[1]);
            }

            if (string.Compare(command, "cat", true) == 0)
            {
                if (strings.Length != 2) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (await OpenFile(strings[1]), string.Empty);
            }

            if (string.Compare(strings[0], "help", true) == 0)
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (ListSupportedCommands(), string.Empty);
            }

            if (string.Compare(strings[0], "leaderboard", true) == 0)
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (ListLeaderBoard(), string.Empty);
            }
            //TODO:
            return (string.Empty, string.Empty);
        }

        private string ListLeaderBoard()
        {
            throw new NotImplementedException();
        }

        private async Task<string> OpenFile(string file)
        {
            //check children for file
            return await _locationService.GetFileInDirectory(file);
        }

        private async Task<(string html, string directory)> ChangeDirectory(string directory)
        {
            if(string.Compare(directory, "..", true) == 0)
                return (string.Empty, await _locationService.TraverseUp());
            else
            {
                return await _locationService.ChangeDirectory(directory);
            }
        }

        private (string, string) ListDirectory()
        {
            return (_locationService.GetFilesInDirectory(), string.Empty); 
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
    }
}
