using DiscordTwitchBot.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTwitchBot.Tests.Logging;

// TODO : WHY ARE THESE NOT BEING DETECTED?!?!?!?!

public class LoggingExtensionsTests
{
    [Fact]
    public void LoggingExtension_ConfiguresApplicationLogging()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        using var host = builder.Build();
        var options = host.Services.GetRequiredService<IOptions<LoggerFilterOptions>>().Value;

        // Assert
        Assert.Equal(LogLevel.Warning, options.MinLevel);

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
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        using var host = builder.Build();
        var options = host.Services.GetRequiredService<IOptions<LoggerFilterOptions>>().Value;
        LoggerFilterRule? rule = options.Rules
            .FirstOrDefault(rule =>
                rule.CategoryName == "DiscordTwitchBot");

        // Assert
        Assert.NotNull(rule);
        Assert.Equal(LogLevel.Debug, rule.LogLevel); // In development, the log level for "DiscordTwitchBot" should be Debug
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
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Act
        builder.Logging.AddLogging(builder.Environment);
        using var host = builder.Build();
        var options = host.Services.GetRequiredService<IOptions<LoggerFilterOptions>>().Value;
        LoggerFilterRule? rule = options.Rules
            .FirstOrDefault(rule =>
                rule.CategoryName == "DiscordTwitchBot");

        // Assert
        Assert.NotNull(rule);
        Assert.Equal(LogLevel.Information, rule.LogLevel); // In production, the log level for "DiscordTwitchBot" should be Information
    }
}
