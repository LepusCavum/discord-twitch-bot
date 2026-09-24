using System.Reflection;
using DiscordTwitchBot.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTwitchBot.Services;

// <summary>
// Represents a service that handles startup operations for the bot application.
// </summary>
public class StartupService : IStartupService, IHostedService, IDisposable
{
    private static readonly EventId ApplicationStartingEvent = new(1000, "ApplicationStarting");
    private static readonly EventId ServiceStartingEvent = new(1001, "ServiceStarting");
    private static readonly EventId ApplicationStoppingEvent = new(1002, "ApplicationStopping");
    private static readonly EventId ApplicationStoppedEvent = new(1003, "ApplicationStopped");
    private static readonly EventId ServiceStoppedEvent = new(1004, "ServiceStopped");
    private static readonly EventId StartupFailedEvent = new(1005, "StartupFailed");

    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<StartupService> _logger;
    private readonly IHostEnvironment _environment;
    private readonly IOptions<ApplicationOptions> _options;
    private CancellationTokenRegistration _stoppingRegistration;
    private CancellationTokenRegistration _stoppedRegistration;
    private int _callbacksRegistered;

    public StartupService(IHostApplicationLifetime applicationLifetime, 
        ILogger<StartupService> logger, IHostEnvironment environment, 
        IOptions<ApplicationOptions> options)
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
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            _logger.LogInformation(ApplicationStartingEvent,
                "Application starting: {ApplicationName} v{Version} in {Environment}. App cancellation requested? {tokenRequested}",
                _environment.ApplicationName, 
                version, 
                _options.Value.Environment, _applicationLifetime.ApplicationStopping.IsCancellationRequested);
            _logger.LogInformation(ServiceStartingEvent,
                "StartupService is starting. Service cancellation requested? {tokenRequested}",
                cancellationToken.IsCancellationRequested);

            if (Interlocked.Exchange(ref _callbacksRegistered, 1) == 0)
            {
                _stoppingRegistration = _applicationLifetime.ApplicationStopping.Register(() =>
                    _logger.LogInformation(ApplicationStoppingEvent,
                        "Application is stopping. App cancellation requested? {tokenRequested}",
                        _applicationLifetime.ApplicationStopping.IsCancellationRequested));
                _stoppedRegistration = _applicationLifetime.ApplicationStopped.Register(() =>
                    _logger.LogInformation(ApplicationStoppedEvent,
                        "Application is stopped. App cancellation requested? {tokenRequested}",
                        _applicationLifetime.ApplicationStopping.IsCancellationRequested));
            }

            return Task.CompletedTask;
        } catch (Exception ex) {
            _logger.LogError(StartupFailedEvent, ex,
                "Application startup failed during {StartupStage}.", "StartupService.StartAsync");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(ServiceStoppedEvent,
            "StartupService is stopped. Service cancellation requested? {tokenRequested}", 
            cancellationToken.IsCancellationRequested);
                
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _stoppingRegistration.Dispose();
        _stoppedRegistration.Dispose();
    }
}