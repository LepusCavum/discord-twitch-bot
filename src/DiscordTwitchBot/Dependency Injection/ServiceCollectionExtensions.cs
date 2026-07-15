using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTwitchBot.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotServices(this IServiceCollection services)
    {
        services.AddSingleton<StartupService>();
        
        return services;
    }
}