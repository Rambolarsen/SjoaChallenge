using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SjoaChallenge;
using SjoaChallenge.Services;
using SjoaChallenge.Utilities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddSingleton(typeof(IUsernameGenerator), typeof(UsernameGenerator));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<CommandService>();

await builder.Build().RunAsync();
