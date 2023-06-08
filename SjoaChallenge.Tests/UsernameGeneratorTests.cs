using FluentAssertions;
using RichardSzalay.MockHttp;
using SjoaChallenge.Services;
using SjoaChallenge.Utilities;
using System.Text.Json;

namespace SjoaChallenge.Tests
{
    [TestFixture]
    public class UsernameGeneratorTests
    {

        private UsernameGenerator? _sut;

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();

            var actions = new List<string>() { "eats", "sits on top of", "picks"};
            mockHttp.When("http://localhost/json/actions.json")
                    .Respond("application/json", JsonSerializer.Serialize(actions));

            var animals = new List<string>() { "cat dog", "cow", "dog" };
            mockHttp.When("http://localhost/json/animals.json")
                    .Respond("application/json", JsonSerializer.Serialize(animals));

            var food = new List<string>() { "banana peel", "apple", "cake" };
            mockHttp.When("http://localhost/json/food.json")
                    .Respond("application/json", JsonSerializer.Serialize(food));

            var client = new HttpClient(mockHttp)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var jsreader = new JsonReader(client);
            _sut = new UsernameGenerator(jsreader);
        }

        [Test]
        public void OnInitialize_ShouldNotBeNull()
        {
            _sut.Should().NotBeNull();
        }

        [Test]
        public async Task OnGenerateUsername_ShouldGenerateUsername() 
        {
            var username = await _sut!.GenerateUsername();
            username.Should().NotBeNull();
        }

        [Test]
        public async Task OnGenerateUsername_GeneratedUsername_ShouldNotContainSpaces()
        {
            var username = await _sut!.GenerateUsername();
            username.Should().NotContain(" ");
        }
    }
}
