using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiscordTwitchBot.Hosting;
using DiscordTwitchBot.Services;
using DiscordTwitchBot.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    [Fact]
    public async Task Host_ApplicationStoppingToken_IsCancelledWhenHostStops()
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

    [Fact]
    public async Task StartupService_IsRegisteredAsHostedService()
    {
        // Arrange
        using var host = BotHost.Create();

        // Act
        var hostedServices = host.Services.GetServices<IHostedService>();
        var startupService = hostedServices.OfType<StartupService>().FirstOrDefault();

        // Assert
        Assert.NotNull(startupService); 
    }

    [Fact]
    public async Task Host_ExecutesStartupServiceStartAsync()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddBotServices(builder.Configuration);
        builder.Services.AddSingleton<ILogger<StartupService>>(logger);

        using var host = builder.Build();

        // Act
        await host.StartAsync();

        // Assert
        Assert.Contains(
            logger.Entries,
            log => log.Message.Contains("Application starting:")
        );
    }

    [Fact]
    public async Task StartupService_ObservesApplicationStopping_WithoutException()
    {
        // Arrange
        using var host = BotHost.Create();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        var cancellationToken = lifetime.ApplicationStopping;
        
        await host.StartAsync(); // Start the host in the background

        // Act
        var exception = await Record.ExceptionAsync(() => host.StopAsync());

        // Assert
        Assert.Null(exception);
        Assert.True(cancellationToken.IsCancellationRequested);
    }
}