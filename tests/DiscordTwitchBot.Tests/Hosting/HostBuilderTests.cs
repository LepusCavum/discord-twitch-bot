using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiscordTwitchBot.Hosting;
using DiscordTwitchBot.Services;
using DiscordTwitchBot.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        var startupService = host.Services.GetRequiredService<IHostedService>();

        // Assert
        Assert.NotNull(startupService);
    }

    [Fact]
    public void Host_RegistersStartupService_ThroughDependencyInjection()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddBotServices(builder.Configuration);

        using var host = builder.Build();

        // Act
        var startupService = host.Services.GetRequiredService<IStartupService>();
        var hostedServices = host.Services.GetServices<IHostedService>();

        // Assert
        Assert.NotNull(startupService);
        Assert.Contains(hostedServices, service => service is StartupService);
    }

    [Fact]
    public async Task Host_ValidateOnStart_Succeeds_WithValidRegistrations()
    {
        // Arrange
        var startupLogger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = "DiscordTwitchBot",
            ["Application:Environment"] = "Test",
            ["Application:Version"] = "0.1.0"
        });
        builder.Logging.AddProvider(startupLogger);
        builder.ConfigureContainer(
            new DefaultServiceProviderFactory(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            }),
            _ => { });
        builder.Services.AddBotServices(builder.Configuration);
        using var host = builder.Build();

        // Act
        var exception = await Record.ExceptionAsync(() => host.StartAsync());

        // Assert
        Assert.Null(exception);
        Assert.Contains(startupLogger.Entries, log => log.EventId.Id == 1006);
    }

    [Fact]
    public async Task Host_ValidateOnStart_Fails_WhenOptionsAreInvalid()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = null
        });

        builder.Services.AddBotServices(builder.Configuration);
        using var host = builder.Build();

        // Act
        var exception = await Record.ExceptionAsync(() => host.StartAsync());

        // Assert
        var validationException = Assert.IsType<OptionsValidationException>(exception);
        Assert.Contains(
            validationException.Failures,
            failure => failure.Contains("Application name is required.", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Host_ValidateOnStart_FailsBeforeRuntimeHostedServicesStart()
    {
        // Arrange
        var logger = new TestLogger<StartupService>();
        var runtimeService = new TrackingHostedService();
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = null
        });
        builder.Logging.AddProvider(logger);
        builder.Services.AddBotServices(builder.Configuration);
        builder.Services.AddSingleton<IHostedService>(runtimeService);

        using var host = builder.Build();

        // Act
        var exception = await Record.ExceptionAsync(() => host.StartAsync());

        // Assert
        var validationException = Assert.IsType<OptionsValidationException>(exception);
        Assert.Contains(
            validationException.Failures,
            failure => failure.Contains("Application name is required.", StringComparison.Ordinal));
        Assert.False(runtimeService.Started);
        Assert.DoesNotContain(logger.Entries, log => log.EventId.Id == 1006);
    }

    [Fact]
    public void Host_ValidateOnBuild_Fails_WhenRequiredDependencyIsUnavailable()
    {
        // Arrange
        var startupLogger = new TestLogger<StartupService>();
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.AddProvider(startupLogger);
        builder.ConfigureContainer(
            new DefaultServiceProviderFactory(new ServiceProviderOptions { ValidateOnBuild = true }),
            _ => { });
        builder.Services.AddSingleton<UnavailableDependencyConsumer>();

        // Act
        var exception = Record.Exception(() => builder.Build());

        // Assert
        Assert.NotNull(exception);
        Assert.Contains("RequiresUnavailableDependency", exception.ToString());
        Assert.DoesNotContain(startupLogger.Entries, log => log.EventId.Id == 1006);
    }

    [Fact]
    public async Task Host_DoesNotStartLaterHostedService_WhenEarlierHostedServiceFails()
    {
        // Arrange
        var laterService = new TrackingHostedService();
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddHostedService<FailingHostedService>();
        builder.Services.AddSingleton<IHostedService>(laterService);

        using var host = builder.Build();

        // Act
        var exception = await Record.ExceptionAsync(() => host.StartAsync());

        // Assert
        Assert.NotNull(exception);
        Assert.False(laterService.Started);
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