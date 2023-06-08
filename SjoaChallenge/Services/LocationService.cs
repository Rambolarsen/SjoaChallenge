using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SjoaChallenge.Utilities;
using System.Text;

namespace SjoaChallenge.Services
{
    public class LocationService : IAsyncInitialization
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IJsonReader _jsonReader;
        private Structure? _fileStructure;
        private Structure? _currentDirectory;
        private const string Directory = "Directory";
        private const string File = "File";

        public Task Initialization { get; private set; }

        public LocationService(ILocalStorageService localStorage,
            IJsonReader jsonReader)
        {
            _localStorage = localStorage;
            _jsonReader = jsonReader;
            Initialization = Init();
        }
        private async Task Init()
        {
            _fileStructure = await _jsonReader.GetJson<Structure?>("filestructure");
            _currentDirectory = _fileStructure;
            await _localStorage.SetItemAsync(Directory, _currentDirectory!.Url);
        }

        public async Task<string> GetDirectory()
        {
            var location = (await _localStorage.GetItemAsync<string>(Directory)) ?? string.Empty;
            if (string.IsNullOrEmpty(location))
            {
                return await ChangeDirectory();
            }

            return location;
        }

        public async Task<string> ChangeDirectory()
        {
            await _localStorage.SetItemAsync(Directory, _currentDirectory!.Url);
            return await _localStorage.GetItemAsync<string>(Directory);
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
                x.Name.EqualsIgnoreCase(directory) 
                && x.Type.EqualsIgnoreCase(Directory));
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
                    var name = structure.Type.EqualsIgnoreCase("directory") ? structure.Name + "/" : structure.Name;
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
                x.Name.EqualsIgnoreCase(file)
                && x.Type.EqualsIgnoreCase(File));
            if (current == null) return "<p>Invalid file. You're only able to open files in current directory.</p>";

            var responseFile = await _jsonReader.GetJson<Challenge?>(file);

            return responseFile?.HTML ?? string.Empty;
        }
    }
}
