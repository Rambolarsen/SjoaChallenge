using SjoaChallenge.Utilities;
using System.Net.Http.Json;
using System.Text;

namespace SjoaChallenge.Services
{
    public class CommandService : IAsyncInitialization
    {
        private readonly IJsonReader _jsonReader;
        private readonly LocationService _locationService;
        private readonly LeaderboardService _leaderboardService;
        private readonly SolveService _solveService;
        private ICollection<string>? _supportedCommands;

        public CommandService(IJsonReader jsonReader, 
            LocationService locationService,
            LeaderboardService leaderboardService,
            SolveService solveService)
        {
            _jsonReader = jsonReader;
            _locationService = locationService;
            _leaderboardService = leaderboardService;
            _solveService = solveService;

            Initialization = Init();
        }

        public Task Initialization { get; set; }

        private async Task Init()
        {
            await _locationService.Initialization;
            _supportedCommands = await _jsonReader.GetJson<ICollection<string>>("supportedCommands");
        }

        public async Task<(string html,string directory)> HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) { return ("<p>No command given.</p>", string.Empty); }

            var strings = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var command = strings[0].ToLowerInvariant();
            if (!_supportedCommands!.Contains(command))
            {
                return ("<p> Unsupported command. Write 'help' to see supported commands.</p>", string.Empty);
            }

            if (command.EqualsIgnoreCase("ls") || command.EqualsIgnoreCase("dir"))
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return ListDirectory();
            }

            if (command.EqualsIgnoreCase("cd"))
            {
                if (strings.Length != 2) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return await ChangeDirectory(strings[1]);
            }

            if (command.EqualsIgnoreCase("cat"))
            {
                if (strings.Length != 2) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (await OpenFile(strings[1]), string.Empty);
            }

            if (command.EqualsIgnoreCase("help"))
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (ListSupportedCommands(), string.Empty);
            }

            if (command.EqualsIgnoreCase("leaderboard"))
            {
                if (strings.Length != 1) return ("<p>Invalid number of arguments.</p>", string.Empty);
                return (await ListLeaderBoard(), string.Empty);
            }

            if (command.EqualsIgnoreCase("solve"))
            {
                if (strings.Length == 1) return ("<p>Invalid number of arguments.</p>.", string.Empty);
                return (await Solve(strings.Skip(1)), string.Empty);
            }
            //TODO:
            return (string.Empty, string.Empty);
        }

        private async Task<string> Solve(IEnumerable<string> strings)
        {
            var phrase = string.Join(" ", strings);
            phrase = phrase.Trim();
            var currentTask = await _locationService.GetDirectory();
            var solved = await _solveService.TrySolve(currentTask, phrase);
            if (solved)
            {
                return "<p>Congratulatons! That was the correct answer!</p>";
            }

            return "<p>Wrong answer! Try again.</p>";
        }

        private async Task<string> ListLeaderBoard()
        {
            var leaderboard = await _leaderboardService.GetLeaderboard();
            var sortedLeaderboard = (from entry in leaderboard orderby entry.Value.Item1 descending orderby entry.Value.Item2 ascending select entry).ToList();
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine("<p>Current Leaderboard:</p>");
            stringbuilder.AppendLine("<ul style='list-style-type: none;'>");
            foreach (var user in leaderboard)
            {
                stringbuilder.AppendLine($"<li>{user.Key} - {user.Value.score}</li>");
            }
            stringbuilder.AppendLine("</ul>");

            return stringbuilder.ToString();
        }

        private async Task<string> OpenFile(string file)
        {
            //check children for file
            return await _locationService.GetFileInDirectory(file);
        }

        private async Task<(string html, string directory)> ChangeDirectory(string directory)
        {
            if(directory.EqualsIgnoreCase(".."))
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
