# Project documentation

This folder contains the operational and architectural reference material for the repository. The root README remains a quick-start summary; the docs in this folder capture the current implementation details and project conventions in a way that is easier to maintain as the application evolves.

## Contents

- [Application architecture](architecture/application-host.md)
- [Configuration and secrets](configuration/secrets.md)
- [Continuous integration](ci/github-actions.md)
- [Development and local runtime setup](development/local-run.md)
- [Contribution conventions](contributing/conventions.md)
- [Testing overview](testing/test-overview.md)

## Current project snapshot

The application is a .NET 10 console application built on the Generic Host pattern. The startup path is intentionally thin and delegated to the host builder and startup service, while configuration and logging are centralized in their own extension points.

Key implementation details from the current codebase:

- Target framework: .NET 10 (`net10.0`)
- Entry point: [src/DiscordTwitchBot/Program.cs](../src/DiscordTwitchBot/Program.cs)
- Host factory: [src/DiscordTwitchBot/Hosting/BotHost.cs](../src/DiscordTwitchBot/Hosting/BotHost.cs)
- Application settings model: [src/DiscordTwitchBot/Configuration/ApplicationOptions.cs](../src/DiscordTwitchBot/Configuration/ApplicationOptions.cs)
- Service registration: [src/DiscordTwitchBot/Dependency Injection/ServiceCollectionExtensions.cs](../src/DiscordTwitchBot/Dependency%20Injection/ServiceCollectionExtensions.cs)
- Startup lifecycle service: [src/DiscordTwitchBot/Services/StartupService.cs](../src/DiscordTwitchBot/Services/StartupService.cs)
- Logging configuration: [src/DiscordTwitchBot/Logging/LoggingExtensions.cs](../src/DiscordTwitchBot/Logging/LoggingExtensions.cs)
- Runtime defaults: [src/DiscordTwitchBot/appsettings.json](../src/DiscordTwitchBot/appsettings.json)

## Intended usage of this folder

Use these pages when you need more detail about:

- how the host is assembled and started,
- which configuration values are expected,
- how local development and CI environment settings work,
- how to validate changes with the current automated test suite,
- or how branch and PR conventions are expected to be followed.

## Related project docs

- [README](../README.md)
- [Issue and planning docs](plans/)
