# Application architecture

This repository currently implements a very small .NET Generic Host application. The design favors a thin entry point, centralized configuration, and a single startup lifecycle service that validates the application state before the host is considered ready.

## Startup flow

The startup flow is:

1. [src/DiscordTwitchBot/Program.cs](../../src/DiscordTwitchBot/Program.cs) creates the host using `BotHost.Create()`.
2. [src/DiscordTwitchBot/Hosting/BotHost.cs](../../src/DiscordTwitchBot/Hosting/BotHost.cs) builds a host with the required configuration and logging pipeline.
3. [src/DiscordTwitchBot/Dependency Injection/ServiceCollectionExtensions.cs](../../src/DiscordTwitchBot/Dependency%20Injection/ServiceCollectionExtensions.cs) registers `StartupService` as both a singleton and `IHostedService`.
4. [src/DiscordTwitchBot/Services/StartupService.cs](../../src/DiscordTwitchBot/Services/StartupService.cs) runs during host startup and logs the current runtime state.
5. The host continues to run until the application lifetime is canceled.

## Host construction

The host builder uses `Host.CreateApplicationBuilder()` and then:

- resolves the environment from environment variables or configuration,
- clears any default configuration sources,
- loads `appsettings.json`,
- loads an environment-specific settings file if available,
- adds user secrets during Development execution,
- adds environment variables,
- configures the logging pipeline,
- registers bot services.

This design keeps creation centralized and predictable.

## Configuration and validation

The repository uses the Options pattern with `ApplicationOptions` and `ValidateDataAnnotations()`.

The required options are defined here:

- `Application:Name`
- `Application:Environment`
- `Application:Version`

The validation happens at host startup via `ValidateOnStart()`. This ensures configuration failures are detected early and do not reach runtime behavior with partially configured values.

## Logging design

The logging pipeline is provided by [src/DiscordTwitchBot/Logging/LoggingExtensions.cs](../../src/DiscordTwitchBot/Logging/LoggingExtensions.cs).

It does the following:

- clears default providers,
- adds a simple console provider,
- configures debug-level output in Development,
- configures informational output in Production,
- suppresses noisy framework and system logs by filtering them to warnings.

## Lifecycle service

The `StartupService` is the current application-owned lifecycle component. It exposes `StartAsync`, `StopAsync`, and `IDisposable`, and records information about:

- the application start event,
- the application stop event,
- shutdown token state,
- successful startup validation,
- startup failure paths.

This service is deliberately lightweight and acts as the canonical validation gate for the minimum viable host setup.

## Current scope note

The codebase is intentionally minimal and does not currently implement Twitch or Discord application integrations. The architecture reflects a foundation for future service expansion without broadening the current scope beyond the host and configuration lifecycle.

## Related references

- [Project documentation index](../README.md)
- [Configuration and secrets](../configuration/secrets.md)
- [Development setup](../development/local-run.md)
