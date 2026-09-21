using DiscordTwitchBot.Configuration;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTwitchBot.DependencyInjection;

public static class ServiceCollectionExtensions
{
    // <summary>
    // Extension method to add bot services to the IServiceCollection.
    // </summary>
    // <param name="services">The IServiceCollection to add services to.</param>
    // <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddBotServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<StartupService>();
        services.AddSingleton<IStartupService>(serviceProvider => serviceProvider.GetRequiredService<StartupService>());
        services.AddHostedService(serviceProvider => serviceProvider.GetRequiredService<StartupService>());

        services.AddOptions<ApplicationOptions>()
            .Bind(configuration.GetSection("Application"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}