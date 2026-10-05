# Development and local runtime setup

This project is a .NET 10 console application that runs through the Generic Host model created in [src/DiscordTwitchBot/Hosting/BotHost.cs](../../src/DiscordTwitchBot/Hosting/BotHost.cs).

## Local workflow

From the repository root, the app can be launched with:

```bash
DOTNET_ENVIRONMENT=Development dotnet run --project src/DiscordTwitchBot
```

For the production configuration:

```bash
DOTNET_ENVIRONMENT=Production dotnet run --project src/DiscordTwitchBot
```

The environment name is resolved in the host builder using `DOTNET_ENVIRONMENT`, then `ASPNETCORE_ENVIRONMENT`, then the `Application:Environment` configuration value, or `Production` as a fallback.

## Important runtime behavior

The startup path is intentionally thin:

- [src/DiscordTwitchBot/Program.cs](../../src/DiscordTwitchBot/Program.cs) creates and runs the host.
- [src/DiscordTwitchBot/Hosting/BotHost.cs](../../src/DiscordTwitchBot/Hosting/BotHost.cs) loads configuration, sets the environment, registers services, and builds the host.
- [src/DiscordTwitchBot/Dependency Injection/ServiceCollectionExtensions.cs](../../src/DiscordTwitchBot/Dependency%20Injection/ServiceCollectionExtensions.cs) wires in the application options and hosted startup service.
- [src/DiscordTwitchBot/Services/StartupService.cs](../../src/DiscordTwitchBot/Services/StartupService.cs) logs startup and shutdown lifecycle information and validates the application is configured correctly.

## Logging setup

The project clears the default providers and adds a simple console logger. Development mode enables debug-level output for the bot namespace and warnings for framework namespaces, while Production mode raises the minimum level for bot logs to information.

See [src/DiscordTwitchBot/Logging/LoggingExtensions.cs](../../src/DiscordTwitchBot/Logging/LoggingExtensions.cs) for the exact filtering behavior.

## Example environment shell configuration

```bash
echo 'export MY_VAR="value"' >> ~/.bashrc
source ~/.bashrc
```

This pattern is useful for setting operational environment values outside the repository, but the project does not currently rely on repo-local `.env` files.

## Related references

- [Configuration and secrets](../configuration/secrets.md)
- [Continuous integration](../ci/github-actions.md)
- [Project README](../../README.md)
