using DiscordTwitchBot.Configuration;
using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DiscordTwitchBot.Tests.DependencyInjection;

public class ServiceRegistrationTests
{
    // This test checks if the StartupService can be resolved from the DI container after registering services.
    [Fact] // tells xUnit that this is a test to be executed. Would be ignored without
    public void StartupService_ShouldResolveSuccessfully()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddBotServices(builder.Configuration); // extension method to register all services
        
        var provider = builder.Services.BuildServiceProvider(); // creates the DI container with host services

        // Act
        var startupService = provider.GetRequiredService<IStartupService>(); // try to resolve the service

        // Assert
        Assert.NotNull(startupService); // Did it build successfully?
        Assert.IsType<StartupService>(startupService); // Is it the correct type?
    }

    // This test verifies exceptions are thrown if StartupService is not registered
    [Fact]
    public void StartupService_ShouldThrow_WhenNotRegistered()
    {
        // Arrange
        var services = new ServiceCollection(); // creates DI container registration area
        // Note: We are NOT calling AddBotServices() here, so StartupService is not registered.
        
        var provider = services.BuildServiceProvider(); // creates the DI container

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<StartupService>()); // Expecting an exception because StartupService is not registered
    }

    [Fact]
    public void StartupService_ShouldResolve_WithApplicationOptions()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = "TestApp"
        });


        builder.Services.AddBotServices(builder.Configuration);

        // Act
        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ApplicationOptions>>();
        var startupService = provider.GetRequiredService<IStartupService>(); 

        // Assert
        Assert.Equal("TestApp", options.Value.Name);
        Assert.NotNull(startupService);
        Assert.IsType<StartupService>(startupService);
    }

    [Fact]
    public void AddBotServices_RegistersBoundApplicationOptions()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = "TestApp"
        });
        builder.Services.AddBotServices(builder.Configuration);

        // Act
        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ApplicationOptions>>();

        // Assert
        Assert.Equal("TestApp", options.Value.Name);
    }
}
