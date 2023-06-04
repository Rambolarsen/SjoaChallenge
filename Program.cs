using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using SjoaChallenge;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using SjoaChallenge.Utilities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddScoped<UsernameGenerator>();

var appsettings = new Dictionary<string, string?>()
{
   { "API:Key", "DetteErEnDårligLøsningFordiJegHarDårligTid" }
};

var config = new MemoryConfigurationSource
{
    InitialData = appsettings
};

builder.Configuration.Add(config);

await builder.Build().RunAsync();
