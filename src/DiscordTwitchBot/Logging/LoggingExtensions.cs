using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DiscordTwitchBot.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder logging, IHostEnvironment env)
    {
        logging.ClearProviders(); // Clear existing logging providers
        logging.AddSimpleConsole(options => // Add console logging provider
        {
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "; // Set timestamp format for log messages
            options.ColorBehavior = LoggerColorBehavior.Enabled; // Enable colored output in the console
        });


        if (env.IsDevelopment())
        {
            ConfigureDevelopment(logging); // Add development-specific logging configuration here
        } else if (env.IsProduction())
        {
            ConfigureProduction(logging); // Add production-specific logging configuration here
        }

        return logging;
    }

    public static ILoggingBuilder ConfigureDevelopment(this ILoggingBuilder logging)
    {
        logging.SetMinimumLevel(LogLevel.Warning); // Set minimum log level to Debug for development
        
        logging.AddFilter(
            "DiscordTwitchBot", LogLevel.Debug)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);

        return logging;
    }

    public static ILoggingBuilder ConfigureProduction(this ILoggingBuilder logging)
    {
        logging.SetMinimumLevel(LogLevel.Warning); // Set minimum log level to Debug for development
        
        logging.AddFilter(
            "DiscordTwitchBot", LogLevel.Information)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);

        return logging;
    }
}