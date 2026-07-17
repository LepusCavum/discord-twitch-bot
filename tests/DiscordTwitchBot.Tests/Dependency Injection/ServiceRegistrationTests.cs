using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTwitchBot.Tests.DependencyInjection;

public class ServiceRegistrationTests
{
    [Fact] // tells xUnit that this is a test to be executed. Would be ignored without
    public void ApplicationServices_ShouldResolveSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection(); // creates DI container registration area
        services.AddBotServices(); // extension method to register all services
        
        var provider = services.BuildServiceProvider(); // creates the DI container

        // Act
        var startupService = provider.GetRequiredService<StartupService>(); // try to resolve the service

        // Assert
        Assert.NotNull(startupService); // Did it build successfully?
    }
}
