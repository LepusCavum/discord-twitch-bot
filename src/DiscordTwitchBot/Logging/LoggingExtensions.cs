using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTwitchBot.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder logging, IHostEnvironment environment)
    {
        logging.ClearProviders(); // Clear existing logging providers
        logging.AddConsole(); // Add console logging provider

        if (environment.IsDevelopment())
        {
            //ConfigureDevelopment(logging); // Add development-specific logging configuration here
        } else if (environment.IsProduction())
        {
            //ConfigureProduction(logging); // Add production-specific logging configuration here
        }

        return logging;
    }

    public static ILoggingBuilder ConfigureDevelopment(this ILoggingBuilder logging)
    {
        // Add development-specific logging configuration here
        logging.SetMinimumLevel(LogLevel.Warning); // Set minimum log level to Debug for development
        
        logging.AddFilter(
            "DiscordTwitchBot", LogLevel.Debug)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);

        return logging;
    }

    public static ILoggingBuilder ConfigureProduction(this ILoggingBuilder logging)
    {
        // Add production-specific logging configuration here
        logging.SetMinimumLevel(LogLevel.Warning); // Set minimum log level to Debug for development
        
        logging.AddFilter(
            "DiscordTwitchBot", LogLevel.Information)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);

        return logging;
    }
}