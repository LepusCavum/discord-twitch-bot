using Microsoft.Extensions.Hosting;

public sealed class ThrowingApplicationLifetime(Exception exception) : IHostApplicationLifetime
{
    public CancellationToken ApplicationStarted => CancellationToken.None;

    public CancellationToken ApplicationStopping => throw exception;

    public CancellationToken ApplicationStopped => CancellationToken.None;

    public void StopApplication()
    {
    }
}