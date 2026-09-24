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

        var serviceProviderOptions = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        builder.ConfigureContainer(
            new DefaultServiceProviderFactory(serviceProviderOptions),
            services => { });

        var configuredEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? builder.Configuration["Application:Environment"]
            ?? "Production";

        Console.WriteLine($"Environment: {builder.Configuration["Application:Environment"]}"); // Log the configured environment for debugging

        builder.Environment.EnvironmentName = configuredEnvironment;

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory) // Set the base path for configuration files
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false) // Load base configuration from appsettings.json
            .AddJsonFile($"appsettings.{configuredEnvironment}.json", optional: true, reloadOnChange: false); // Load environment-specific override configuration when present

        builder.Configuration["Application:Environment"] = configuredEnvironment;

        builder.Logging.AddLogging(builder.Environment); // Configure logging using the extension method

        builder.Services.AddBotServices(builder.Configuration); // Register bot services using the extension method

        return builder.Build(); // Build and return the configured host
    }
}