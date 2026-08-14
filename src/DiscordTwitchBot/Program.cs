using DiscordTwitchBot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = BotHost.Create(); // Create and configure the host for the bot application

await host.RunAsync(); // Run the host, which will keep the application running until it is stopped