using DiscordTwitchBot.Configuration;
using Microsoft.Extensions.Configuration;

namespace DiscordTwitchBot.Tests.Configuration;

public class ApplicationOptionsTests
{
    [Fact]
    public void ApplicationOptions_BindsNameFromConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ApplicationOptions:Name", "TestApp" }
            })
            .Build();

        // Act
        var options = new ApplicationOptions();
        configuration.Bind("ApplicationOptions", options);

        // Assert
        Assert.Equal("TestApp", options.Name);        
    }
}