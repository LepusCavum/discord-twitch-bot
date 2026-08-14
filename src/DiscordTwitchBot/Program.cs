using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = BotHost.Create(); // Create and configure the host for the bot application
var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
var cancellationToken = lifetime.ApplicationStopping;
var startupService = host.Services.GetRequiredService<IStartupService>(); // Resolve the IStartupService from the host's service provider

// await startupService.StartAsync(cancellationToken); // Start the startup service asynchronously
await host.RunAsync(); // Run the host, which will keep the application running until it is stopped