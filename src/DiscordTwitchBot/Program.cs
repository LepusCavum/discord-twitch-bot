using DiscordTwitchBot.DependencyInjection;
using DiscordTwitchBot.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection(); 

services.AddBotServices(); 

using var serviceProvider = services.BuildServiceProvider(); // "using" ensures that the service provider is disposed of properly when done

var startupService = serviceProvider.GetRequiredService<StartupService>();

Console.WriteLine("StartupService resolved successfully: " + (startupService != null));
if (startupService != null)
{
    startupService.Start();
}