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
    private static void WriteUserSecret(string key, string value)
    {
        var appDataRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var secretDirectory = Path.Combine(appDataRoot, "Microsoft", "UserSecrets", "discord-twitch-bot-user-secrets");
        Directory.CreateDirectory(secretDirectory);

        File.WriteAllText(
            Path.Combine(secretDirectory, "secrets.json"),
            $"{{\"{key.Split(':')[0]}\":{{\"{key.Split(':')[1]}\":\"{value}\"}}}}");

        Environment.SetEnvironmentVariable("APPDATA", appDataRoot);
    }

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

    [Fact]
    public void Host_LoadsUserSecrets_WhenDevelopmentIsConfiguredByApplicationEnvironment()
    {
        // Arrange
        var originalAppData = Environment.GetEnvironmentVariable("APPDATA");
        var originalDotNetEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var originalAspNetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        WriteUserSecret("Feature:LocalSecret", "sentinel-secret-value");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);

        try
        {
            using var host = BotHost.Create();
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal("sentinel-secret-value", configuration["Feature:LocalSecret"]);
        }
        finally
        {
            Environment.SetEnvironmentVariable("APPDATA", originalAppData);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", originalDotNetEnvironment);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalAspNetEnvironment);
        }
    }

    [Fact]
    public void Host_UsesEnvironmentVariablesOverJsonAndUserSecretsValues()
    {
        // Arrange
        var originalAppData = Environment.GetEnvironmentVariable("APPDATA");
        var originalDotNetEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var originalAspNetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var originalFeatureValue = Environment.GetEnvironmentVariable("Feature__LocalSecret");

        WriteUserSecret("Feature:LocalSecret", "user-secret-value");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("Feature__LocalSecret", "environment-secret-value");

        try
        {
            using var host = BotHost.Create();
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal("environment-secret-value", configuration["Feature:LocalSecret"]);
        }
        finally
        {
            Environment.SetEnvironmentVariable("APPDATA", originalAppData);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", originalDotNetEnvironment);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalAspNetEnvironment);
            Environment.SetEnvironmentVariable("Feature__LocalSecret", originalFeatureValue);
        }
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