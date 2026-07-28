using Microsoft.Extensions.Hosting;

namespace DiscordTwitchBot.Services;

public class StartupService
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
        _applicationLifetime.ApplicationStopping.Register(() => Console.WriteLine($"Cancellation requested? {cancellationToken.IsCancellationRequested}"));

        Console.WriteLine($"StartupService: Started. Cancellation requested? {cancellationToken.IsCancellationRequested}");

        return Task.CompletedTask;
    }
}