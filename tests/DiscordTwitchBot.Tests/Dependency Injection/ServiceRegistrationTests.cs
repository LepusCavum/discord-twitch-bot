using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordTwitchBot.Tests.DependencyInjection;

public class ServiceRegistrationTests
{
    // This test checks if the StartupService can be resolved from the DI container after registering services.
    [Fact] // tells xUnit that this is a test to be executed. Would be ignored without
    public void StartupService_ShouldResolveSuccessfully()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddBotServices(); // extension method to register all services
        
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
}
