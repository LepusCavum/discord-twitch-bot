// <summary>
// Defines the contract for a service that handles startup operations for the bot application.
// </summary>
public interface IStartupService
{
    // <summary>
    // Starts the startup service asynchronously.
    // </summary>
    // <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task StartAsync(CancellationToken cancellationToken);

}