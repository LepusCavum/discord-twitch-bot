using System.Reflection;
using DiscordTwitchBot.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTwitchBot.Services;

// <summary>
// Represents a service that handles startup operations for the bot application.
// </summary>
public class StartupService : IStartupService
{
    
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<StartupService> _logger;
    private readonly IHostEnvironment _environment;
    private readonly IOptions<ApplicationOptions> _options;

    public StartupService(IHostApplicationLifetime applicationLifetime, 
        ILogger<StartupService> logger, IHostEnvironment environment, IOptions<ApplicationOptions> options)
    {
        _applicationLifetime = applicationLifetime;
        _logger = logger;
        _environment = environment;
        _options = options;
    }

    // <summary>
    // Starts the startup service asynchronously.
    // </summary>
    // <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try {
            _applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation("Application is stopping. Cancellation requested? {tokenRequested}", cancellationToken.IsCancellationRequested));
            _applicationLifetime.ApplicationStopped.Register(() => _logger.LogInformation("Application is stopped."));
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            _logger.LogInformation("Application starting: {ApplicationName} v{Version} in {Environment}. Cancellation requested? {tokenRequested}",
                _environment.ApplicationName, 
                version, 
                _options.Value.Name, cancellationToken.IsCancellationRequested);

            return Task.CompletedTask;
        } catch (Exception ex) {
            _logger.LogError(ex, "Application startup failed during {StartupStage}.", "StartupService.StartAsync");
            throw;
        }
    }
}