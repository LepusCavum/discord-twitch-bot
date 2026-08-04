using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiscordTwitchBot.Hosting;

namespace DiscordTwitchBot.Tests.Hosting;

public class HostBuilderTests
{
    // This test checks if the host can be created successfully using the BotHost.Create() method.
    [Fact]
    public void CreateHost_BuildsSuccessfully()
    {
        // Arrange
        var host = BotHost.Create();

        // Assert
        Assert.NotNull(host);
    }

    // This test checks to see if the host can start successfully
    [Fact]
    public async Task Host_StartsSuccessfully()
    {
        // Arrange
        using var host = BotHost.Create();

        // Act
        var exception = await Record.ExceptionAsync(() => host.StartAsync());

        // Assert
        Assert.Null(exception);
    }

    // This test checks if the host can stop successfully
    [Fact]
    public async Task Host_StopsSuccessfully()
    {
        // Arrange
        using var host = BotHost.Create();
        await host.StartAsync(); // Start the host first

        // Act
        var exception = await Record.ExceptionAsync(() => host.StopAsync());

        // Assert
        Assert.Null(exception);
    }

    // This test checks if the IStartupService can be resolved from the host's service provider after creating the host.
    [Fact]
    public void Host_ResolvesStartupService()
    {
        // Arrange
        var host = BotHost.Create();

        // Act
        var startupService = host.Services.GetRequiredService<IStartupService>();

        // Assert
        Assert.NotNull(startupService);
    }

    // Test to verify StartupService recieved the application shutdown cancellation token
    [Fact]
    public async Task StartAsync_ReceivesCancellationTokenOnShutdown()
    {
        // Arrange
        using var host = BotHost.Create();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        var cancellationToken = lifetime.ApplicationStopping;
        
        await host.StartAsync(); // Start the host in the background
        Assert.False(cancellationToken.IsCancellationRequested); // Ensure the cancellation token is not yet requested

        // Act
        await host.StopAsync(); // Stop the host, which should trigger the ApplicationStopping event

        // Assert
        Assert.True(cancellationToken.IsCancellationRequested);
    }
}