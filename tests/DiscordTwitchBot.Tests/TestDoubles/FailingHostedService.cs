using Microsoft.Extensions.Hosting;

public sealed class FailingHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("startup service failure");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}