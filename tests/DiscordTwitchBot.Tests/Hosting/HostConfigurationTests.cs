using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Hosting;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DiscordTwitchBot.Tests.Hosting;

public class HostConfigurationTests
{
    // This test verifies that the IConfiguration service can be resolved from the host's service provider.
    [Fact]
    public void HostConfiguration_CanResolveIConfiguration()
    {
        // Arrange
        using var host = BotHost.Create();

        // Act
        IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();

        // Assert
        Assert.NotNull(configuration);
    }
    
    // This test verifies that the application name is correctly loaded from the appsettings.json configuration file.
    [Fact]
    public void HostConfiguration_LoadsApplicationNameFromAppSettings()
    {
        // Arrange
        using var host = BotHost.Create();

        // Act
        IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();
        var appName = configuration["Application:Name"] ?? throw new InvalidOperationException("Missing configuration value: Application:Name");

        // Assert
        Assert.Equal("DiscordTwitchBot", appName);
    }

    [Fact]
    public async Task Host_StartFails_WhenAppNameIsEmpty()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = ""
        });

        builder.Services.AddSingleton<TestLogger<StartupService>>(logger);
        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.Build().StartAsync());
        Assert.DoesNotContain(
            logger.Entries,
            log => log.Message.Contains("StartupService is starting")
        );
    }

    [Fact]
    public async Task Host_StartFails_WhenAppNameIsMissing()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = null
        });

        builder.Services.AddSingleton<TestLogger<StartupService>>(logger);
        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.Build().StartAsync());
        Assert.DoesNotContain(
            logger.Entries,
            log => log.Message.Contains("StartupService is starting")
        );

    }
}