using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = BotHost.Create(); // Create and configure the host for the bot application
var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
var cancellationToken = lifetime.ApplicationStopping;
var startupService = host.Services.GetRequiredService<IStartupService>(); // Resolve the IStartupService from the host's service provider

Console.WriteLine("Starting the bot application...");
await startupService.StartAsync(cancellationToken); // Start the startup service asynchronously
Console.WriteLine("BeepBoop: Bot application started. Press Ctrl+C to exit.");
await host.RunAsync(); // Run the host, which will keep the application running until it is stopped
Console.WriteLine("Stopping the bot application...");