using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Logging;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTwitchBot.Hosting;

// TODO: Remove these comments later
// Generic Host -> https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder

public static class BotHost
{
    // <summary>
    // Creates and configures the IHost (Generic Host) for the bot application.
    // </summary>
    // <returns>The configured IHost instance.</returns>
    public static IHost Create()
    {
        var builder = Host.CreateApplicationBuilder(); // Create the host builder

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory) // Set the base path for configuration files
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false); // Load configuration from appsettings.json

        builder.Logging.AddLogging(builder.Environment); // Configure logging using the extension method

        builder.Services.AddBotServices(builder.Configuration); // Register bot services using the extension method

        var host = builder.Build();
        ValidateRequiredServices(host);

        return host; // Build and return the configured host
    }

    public static void ValidateRequiredServices(IHost host)
    {
        var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("BotHost");

        try
        {
            _ = host.Services.GetRequiredService<IStartupService>();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Required startup service registration is missing: IStartupService.");
            throw new InvalidOperationException("Required startup service registration is missing: IStartupService.", ex);
        }

        var hostedServices = host.Services.GetServices<IHostedService>();
        var startupService = hostedServices.OfType<StartupService>().FirstOrDefault();

        if (startupService is null)
        {
            logger.LogError("Required hosted service registration is missing: StartupService.");
            throw new InvalidOperationException("Required hosted service registration is missing: StartupService.");
        }
    }
}