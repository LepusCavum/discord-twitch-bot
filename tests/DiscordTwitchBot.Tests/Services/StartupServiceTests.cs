using DiscordTwitchBot.Configuration;
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
    public async Task StartupService_RequiresApplicationOptions()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var host = Host.CreateApplicationBuilder().Build();

        var options = Options.Create(new ApplicationOptions
        {
            Name = "TestApp"
        });

        var startupService = new StartupService(
            host.Services.GetRequiredService<IHostApplicationLifetime>(),
            logger,
            host.Services.GetRequiredService<IHostEnvironment>(),
            options);
        
        // Act
        await startupService.StartAsync(CancellationToken.None);

        // Assert
        Assert.Contains(
            logger.Entries, 
            log => log.Message.Contains("TestApp"));
    }
}