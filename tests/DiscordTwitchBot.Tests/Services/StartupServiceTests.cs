using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordTwitchBot.Tests.Services;

public class StartupServiceTests
{
    // This test verifies there are no exceptions when the StartupService.StartAsync is called
    [Fact]
    public async Task StartAsync_CompletesSuccessfully()
    {
        var host = Host.CreateApplicationBuilder().Build();
        // Arrange
        var startupService = new StartupService(host.Services.GetRequiredService<IHostApplicationLifetime>());

        // Act
        var exception = await Record.ExceptionAsync(() => startupService.StartAsync(CancellationToken.None));

        // Assert
        Assert.Null(exception);
    }

}