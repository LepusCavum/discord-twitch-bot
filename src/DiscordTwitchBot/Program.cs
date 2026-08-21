using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = BotHost.Create(); // Create and configure the host for the bot application

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.LogError(
        ex,
        "Application startup failure - {ExceptionType}: {ExceptionMessage}",
        ex.GetType().Name,
        ex.Message);

    // throw;
}