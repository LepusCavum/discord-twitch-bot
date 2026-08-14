using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTwitchBot.Services;

// <summary>
// Represents a service that handles startup operations for the bot application.
// </summary>
public class StartupService : IStartupService, IHostedService
{
    
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<StartupService> _logger;
    private readonly IHostEnvironment _environment;

    public StartupService(IHostApplicationLifetime applicationLifetime, 
        ILogger<StartupService> logger, IHostEnvironment environment)
    {
        _applicationLifetime = applicationLifetime;
        _logger = logger;
        _environment = environment;
    }

    // <summary>
    // Starts the startup service asynchronously.
    // </summary>
    // <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try {
            _applicationLifetime.ApplicationStopping.Register(() => _logger.LogInformation("Application is stopping. Cancellation requested? {tokenRequested}", cancellationToken.IsCancellationRequested));
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            _logger.LogInformation("Application starting: {ApplicationName} v{Version} in {Environment}. Cancellation requested? {tokenRequested}",
                _environment.ApplicationName, 
                version, 
                _environment.EnvironmentName, cancellationToken.IsCancellationRequested);

            return Task.CompletedTask;
        } catch (Exception ex) {
            _logger.LogError(ex, "Application startup failed during {StartupStage}.", "StartupService.StartAsync");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application is stopped. Cancellation requested? {tokenRequested}", 
            cancellationToken.IsCancellationRequested);
                
        return Task.CompletedTask;
    }
}