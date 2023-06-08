using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SjoaChallenge.API.Data;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IUserData, UserData>();
        builder.Services.AddSingleton<ILeaderboardData, LeaderboardData>();
        builder.Services.AddSingleton<ISolveData, SolveData>();
    }
}
