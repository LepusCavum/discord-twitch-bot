using DiscordTwitchBot.Services;

namespace DiscordTwitchBot.Tests.Services;

public class StartupServiceTests
{
    [Fact]
    public void StartupService_ShouldCompleteSuccessfully()
    {
        // Arrange
        var startupService = new StartupService();

        // Act
        var exception = Record.Exception(() => startupService.Start()); // Ensure starting does not throw an exception

        // Assert
        Assert.Null(exception); // Ensure no exception was thrown during the method call
    }
}