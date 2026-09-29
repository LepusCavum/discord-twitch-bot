using Microsoft.Extensions.Hosting;

public sealed class TrackingHostedService : IHostedService
{
    public bool Started { get; private set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Started = true;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}