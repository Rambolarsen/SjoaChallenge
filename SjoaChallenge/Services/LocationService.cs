using Blazored.SessionStorage;
using System.IO;
using System.Net.Http.Json;
using System.Text;

namespace SjoaChallenge.Services
{
    public class LocationService
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly IJsonReader _jsonReader;
        private Structure? _fileStructure;
        private Structure? _currentDirectory;
        private const string Directory = "Directory";
        private const string File = "File";

        public LocationService(ISessionStorageService sessionStorage,
            IJsonReader jsonReader)
        {
            _sessionStorage = sessionStorage;
            _jsonReader = jsonReader;
            Init();
        }
        private async void Init()
        {
            _fileStructure = await _jsonReader.GetJson<Structure?>("filestructure");
            _currentDirectory = _fileStructure;
            await _sessionStorage.SetItemAsync(Directory, _currentDirectory!.Url);
        }

        public async Task<string> GetDirectory()
        {
            var location = (await _sessionStorage.GetItemAsync<string>(Directory)) ?? string.Empty;
            if (string.IsNullOrEmpty(location))
            {
                return await ChangeDirectory();
            }

            return location;
        }

        public async Task<string> ChangeDirectory()
        {
            await _sessionStorage.SetItemAsync(Directory, string.Join("/", _currentDirectory!.Url));
            return await _sessionStorage.GetItemAsync<string>(Directory);
        }

        public async Task<string> TraverseUp()
        {
            var parent = _fileStructure!.GetParent(_currentDirectory!);
            if (parent == null) return string.Empty;

            _currentDirectory = parent;
            return await ChangeDirectory();            
        }

        public async Task<(string, string)> ChangeDirectory(string directory)
        {
            var current = _currentDirectory!.Children.FirstOrDefault(x => 
                string.Compare(x.Name, directory, true) == 0 
                && string.Compare(x.Type, Directory, true) == 0);
            if (current == null) return ("<p>Invalid child directory. You're only able to change into child directories.</p>", string.Empty);

            _currentDirectory = current;
            return (string.Empty, await ChangeDirectory());            
        }

        public string GetFilesInDirectory()
        {
            var stringBuilder = new StringBuilder();
            if(_currentDirectory?.Children != null)
            {
                stringBuilder.AppendLine($"<p>Directory of {string.Join("/", _currentDirectory.Url)}</p>");
                stringBuilder.AppendLine("<ul style='list-style-type: none;'>");
                foreach (var structure in _currentDirectory.Children)
                {
                    var name = string.Compare(structure.Type, "directory", true) == 0 ? structure.Name + "/" : structure.Name;
                    stringBuilder.AppendLine($"<li>{name}</li>");
                }
                stringBuilder.AppendLine("</ul>");
            }
            return stringBuilder.ToString();
        }

        public async Task<string> GetFileInDirectory(string file)
        {
            var stringBuilder = new StringBuilder();
            var current = _currentDirectory!.Children.FirstOrDefault(x =>
                string.Compare(x.Name, file, true) == 0
                && string.Compare(x.Type, File, true) == 0);
            if (current == null) return "<p>Invalid file. You're only able to open files in current directory.</p>";

            var responseFile = await _jsonReader.GetJson<Challenge?>(file);

            return responseFile?.HTML ?? string.Empty;
        }
    }
}
