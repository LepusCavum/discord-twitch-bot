using DiscordTwitchBot.Configuration;
using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTwitchBot.Tests.Services;

public class StartupServiceTests
{
    // This test verifies there are no exceptions when the StartupService.StartAsync is called
    [Fact]
    public async Task StartAsync_CompletesSuccessfully()
    {
        var host = Host.CreateApplicationBuilder().Build();
        // Arrange
        var startupService = new StartupService(host.Services.GetRequiredService<IHostApplicationLifetime>(),
            host.Services.GetRequiredService<ILogger<StartupService>>(), host.Services.GetRequiredService<IHostEnvironment>(),
            Options.Create(new ApplicationOptions()));

        // Act
        var exception = await Record.ExceptionAsync(() => startupService.StartAsync(CancellationToken.None));

        // Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async Task StartupService_LogsStartupInformation()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var host = Host.CreateApplicationBuilder().Build();
        var service = new StartupService(host.Services.GetRequiredService<IHostApplicationLifetime>(), 
            logger, host.Services.GetRequiredService<IHostEnvironment>(), 
            Options.Create(new ApplicationOptions()));

        // Act
        await service.StartAsync(CancellationToken.None);

        // Assert
        Assert.Contains(logger.Entries, log => log.Message.Contains("Application starting"));
    }

    [Fact]
    public async Task StartupService_LogsConfiguredVersion()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var host = Host.CreateApplicationBuilder().Build();
        var configuredVersion = "9.8.7";
        var service = new StartupService(
            host.Services.GetRequiredService<IHostApplicationLifetime>(),
            logger,
            host.Services.GetRequiredService<IHostEnvironment>(),
            Options.Create(new ApplicationOptions { Version = configuredVersion }));

        // Act
        await service.StartAsync(CancellationToken.None);

        // Assert
        var startupLog = Assert.Single(logger.Entries, log => log.EventId.Id == 1000);
        Assert.Contains($"v{configuredVersion}", startupLog.Message);
    }

    [Fact]
    public async Task StartupService_RegistersShutdownLoggingOnlyOnce()
    {
        var logger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddBotServices(builder.Configuration);
        builder.Services.AddSingleton<ILogger<StartupService>>(logger);

        using var host = builder.Build();
        var service = host.Services.GetRequiredService<StartupService>();

        await service.StartAsync(CancellationToken.None);
        await service.StartAsync(CancellationToken.None);
        await host.StopAsync();

        var stoppingLogs = logger.Entries.Where(log => log.Message.Contains("Application is stopping")).ToList();
        var stoppedLogs = logger.Entries.Where(log => log.Message.Contains("Application is stopped")).ToList();

        Assert.Single(stoppingLogs);
        Assert.Equal(1002, stoppingLogs[0].EventId.Id);
        Assert.Single(stoppedLogs);
        Assert.Equal(1003, stoppedLogs[0].EventId.Id);
    }

}