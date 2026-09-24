using DiscordTwitchBot.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTwitchBot.Tests.Logging;

public class LoggingExtensionsTests
{
    [Fact]
    public void LoggingExtension_ConfiguresApplicationLogging()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var logger = new TestLogger<LoggingExtensionsTests>();
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        builder.Logging.AddProvider(logger);
        using var host = builder.Build();
        var categoryLogger = host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Other.Component");
        categoryLogger.LogInformation("application information");
        categoryLogger.LogWarning("application warning");

        // Assert
        Assert.DoesNotContain(logger.Entries, entry => entry.Message == "application information");
        Assert.Contains(logger.Entries, entry => entry.Level == LogLevel.Warning && entry.Message == "application warning");
    }

    [Fact]
    public void LoggingExtension_ConfiguresDevelopmentEnvLogging()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings
            {
                EnvironmentName = Environments.Development
            }
        );
        var logger = new TestLogger<LoggingExtensionsTests>();
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        builder.Logging.AddProvider(logger);
        using var host = builder.Build();
        var categoryLogger = host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DiscordTwitchBot.Test");
        categoryLogger.LogDebug("development debug");
        categoryLogger.LogInformation("development information");

        // Assert
        Assert.Contains(logger.Entries, entry => entry.Level == LogLevel.Debug && entry.Message == "development debug");
        Assert.Contains(logger.Entries, entry => entry.Level == LogLevel.Information && entry.Message == "development information");
    }

    [Fact]
    public void LoggingExtension_ConfiguresProductionEnvLogging()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings
            {
                EnvironmentName = Environments.Production
            }
        );
        var logger = new TestLogger<LoggingExtensionsTests>();
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        builder.Logging.AddProvider(logger);
        using var host = builder.Build();
        var categoryLogger = host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DiscordTwitchBot.Test");
        categoryLogger.LogDebug("production debug");
        categoryLogger.LogInformation("production information");
        categoryLogger.LogWarning("production warning");

        // Assert
        Assert.DoesNotContain(logger.Entries, entry => entry.Message == "production debug");
        Assert.Contains(logger.Entries, entry => entry.Level == LogLevel.Information && entry.Message == "production information");
        Assert.Contains(logger.Entries, entry => entry.Level == LogLevel.Warning && entry.Message == "production warning");
    }
}
