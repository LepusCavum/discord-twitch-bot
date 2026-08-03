using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTwitchBot.Tests.Configuration;

public class ConfigurationTests
{
    // This test verifies that the IConfiguration service can be resolved from the host's service provider.
    [Fact]
    public void Configuration_CanResolveIConfiguration()
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
    public void Configuration_LoadsApplicationNameFromAppSettings()
    {
        // Arrange
        using var host = BotHost.Create();

        // Act
        IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();
        var appName = configuration["Application:Name"] ?? throw new InvalidOperationException("Missing configuration value: Application:Name");

        // Assert
        Assert.Equal("DiscordTwitchBot", appName);
    }
}