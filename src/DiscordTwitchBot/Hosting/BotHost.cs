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
        var builder = Host.CreateApplicationBuilder();

        var configuredEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? builder.Configuration["Application:Environment"]
            ?? "Production";

        builder.Environment.EnvironmentName = configuredEnvironment;

        var serviceProviderOptions = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        builder.ConfigureContainer(
            new DefaultServiceProviderFactory(serviceProviderOptions),
            services => { });

        builder.Configuration.Sources.Clear();

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{configuredEnvironment}.json", optional: true, reloadOnChange: false);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>(optional: true);
        }

        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration["Application:Environment"] = configuredEnvironment;

        builder.Logging.AddLogging(builder.Environment); // Configure logging using the extension method

        builder.Services.AddBotServices(builder.Configuration); // Register bot services using the extension method

        return builder.Build(); // Build and return the configured host
    }
}