using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTwitchBot.Services;

// <summary>
// Represents a service that handles startup operations for the bot application.
// </summary>
public class StartupService : IStartupService
{
    
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<StartupService> _logger;

    public StartupService(IHostApplicationLifetime applicationLifetime, ILogger<StartupService> logger)
    {
        _applicationLifetime = applicationLifetime;
        _logger = logger;
    }

    // <summary>
    // Starts the startup service asynchronously.
    // </summary>
    // <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation($"Application is stopping. Cancellation requested? {cancellationToken.IsCancellationRequested}"));
        _applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation($"Cancellation stopped? {cancellationToken.IsCancellationRequested}"));
        
        _logger.LogInformation($"StartupService: Started. Cancellation requested? {cancellationToken.IsCancellationRequested}");

        return Task.CompletedTask;
    }
}