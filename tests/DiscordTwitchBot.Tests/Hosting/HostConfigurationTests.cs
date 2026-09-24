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
    [Fact]
    public void Host_UsesRuntimeEnvironmentVariable_ForEnvironmentName()
    {
        // Arrange
        var previous = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Production");

        try
        {
            using var host = BotHost.Create();

            // Act
            var environment = host.Services.GetRequiredService<IHostEnvironment>();
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal("Production", environment.EnvironmentName);
            Assert.Equal("Production", configuration["Application:Environment"]);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", previous);
        }
    }

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
    public async Task Host_StartFails_WhenAppNameIsInvalid()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = null
        });

        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.Build().StartAsync());
    }

    [Fact]
    public async Task Host_ValidatesVersion_DuringAppStartup()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = "DiscordTwitchBot",
            ["Application:Environment"] = "Test",
            ["Application:Version"] = null
        });

        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        using var host = builder.Build();
        await Assert.ThrowsAsync<OptionsValidationException>(() => host.StartAsync());
    }
}