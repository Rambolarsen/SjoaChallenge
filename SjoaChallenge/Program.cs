using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SjoaChallenge;
using SjoaChallenge.Services;
using SjoaChallenge.Utilities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("API", conf =>
{
    conf.BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(typeof(IUsernameGenerator), typeof(UsernameGenerator));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<CommandService>();
builder.Services.AddScoped(typeof(IJsonReader), typeof(JsonReader));
builder.Services.AddScoped<LeaderboardService>();
builder.Services.AddScoped<SolveService>();

await builder.Build().RunAsync();
