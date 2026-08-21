using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTwitchBot.DependencyInjection;

public static class ServiceCollectionExtensions
{
    // <summary>
    // Extension method to add bot services to the IServiceCollection.
    // </summary>
    // <param name="services">The IServiceCollection to add services to.</param>
    // <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddBotServices(this IServiceCollection services)
    {
        services.AddSingleton<IStartupService, StartupService>(); // Register StartupService as a singleton implementation of IStartupService
        services.AddHostedService<StartupService>(); // Register StartupService as a hosted service

        return services;
    }
}