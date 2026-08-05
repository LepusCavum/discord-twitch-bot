namespace DiscordTwitchBot.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder logging, IHostEnvironment environment)
    {
        logging.ClearProviders(); // Clear existing logging providers
        logging.AddConsole(); // Add console logging provider

        if (environment.IsDevelopment())
        {
            
        } else if (environment.IsProduction())
        {
            // Add production-specific logging configuration here
        }

        return logging;
    }
}