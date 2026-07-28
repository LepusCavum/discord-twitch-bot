using DiscordTwitchBot.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        builder.Services.AddBotServices(); // Register bot services using the extension method

        return builder.Build(); // Build and return the configured host
    }
}