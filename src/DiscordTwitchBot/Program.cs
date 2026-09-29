using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var bootstrapLoggerFactory = LoggerFactory.Create(logging => logging.AddSimpleConsole());
var logger = bootstrapLoggerFactory.CreateLogger<Program>();

try
{
    using var host = BotHost.Create();
    logger = host.Services.GetRequiredService<ILogger<Program>>();
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.LogError(
        ex,
        "Application startup failure - {ExceptionType}: {ExceptionMessage}",
        ex.GetType().Name,
        ex.Message);

    throw;
}