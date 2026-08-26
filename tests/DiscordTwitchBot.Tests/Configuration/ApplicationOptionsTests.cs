using DiscordTwitchBot.Configuration;
using DiscordTwitchBot.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
                { "Application:Name", "TestApp" }
            })
            .Build();

        // Act
        var options = new ApplicationOptions();
        configuration.Bind("Application", options);

        // Assert
        Assert.Equal("TestApp", options.Name);
    }

    [Fact]
    public async Task ApplicationOptions_ValidationFails_WhenAppNameIsEmpty()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = ""
        });

        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.Build().StartAsync());
    }

    [Fact]
    public async Task ApplicationOptions_ValidationFails_WhenAppNameIsNull()
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
    public async Task ApplicationOptions_ValidationFails_WhenAppNameIsWhitespace()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = " "
        });

        builder.Services.AddBotServices(builder.Configuration);

        // Act & Assert
        await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.Build().StartAsync());
    }

    [Fact]
    public async Task ApplicationOptions_ValidationSucceeds_WhenNameIsProvided()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Application:Name"] = "TestApp"
        });

        builder.Services.AddBotServices(builder.Configuration);

        // Act 
        var exception = await Record.ExceptionAsync(() => builder.Build().StartAsync());
        
        // Assert
        Assert.Null(exception);        
    }
}