using Microsoft.Extensions.Hosting;

namespace DiscordTwitchBot.Services;

// <summary>
// Represents a service that handles startup operations for the bot application.
// </summary>
public class StartupService : IStartupService
{
    
    private readonly IHostApplicationLifetime _applicationLifetime;

    public StartupService(IHostApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
    }

    // <summary>
    // Starts the startup service asynchronously.
    // </summary>
    // <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.ApplicationStopping.Register(() => Console.WriteLine($"Cancellation stopping? {cancellationToken.IsCancellationRequested}"));
        _applicationLifetime.ApplicationStopped.Register(() => Console.WriteLine($"Cancellation stopped? {cancellationToken.IsCancellationRequested}"));
        
        Console.WriteLine($"StartupService: Started. Cancellation requested? {cancellationToken.IsCancellationRequested}");

        return Task.CompletedTask;
    }
}