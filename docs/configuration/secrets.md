# Configuration and secrets

This application uses the .NET Generic Host configuration pipeline and binds application settings through the `Application` section. The current codebase explicitly validates configuration at startup using data annotations on `ApplicationOptions`.

## Active configuration model

The runtime configuration model is defined in [src/DiscordTwitchBot/Configuration/ApplicationOptions.cs](../../src/DiscordTwitchBot/Configuration/ApplicationOptions.cs).

The required keys are:

- `Application:Name`
- `Application:Environment`
- `Application:Version`

These values are validated with `[Required]` attributes. If any are missing, the host fails during startup validation before the app is considered ready.

## Runtime configuration source precedence

The host builder in [src/DiscordTwitchBot/Hosting/BotHost.cs](../../src/DiscordTwitchBot/Hosting/BotHost.cs) sets the runtime environment in this order:

1. `DOTNET_ENVIRONMENT`
2. `ASPNETCORE_ENVIRONMENT`
3. `Application:Environment` from configuration
4. fallback to `Production`

Then it loads settings using this sequence:

1. `appsettings.json`
2. `appsettings.{Environment}.json` if present
3. user secrets when running in Development
4. environment variables

This means configuration resolution follows standard .NET Host behavior while preserving the project's current structure.

## Default appsettings

The repository includes [src/DiscordTwitchBot/appsettings.json](../../src/DiscordTwitchBot/appsettings.json), which currently defines:

```json
{
  "Application": {
    "Name": "DiscordTwitchBot",
    "Version": "0.1.0",
    "Environment": "Development"
  }
}
```

This is the baseline configuration used when the environment-specific file is missing or not applicable.

## Local development with User Secrets

The project file declares a `UserSecretsId` in [src/DiscordTwitchBot/DiscordTwitchBot.csproj](../../src/DiscordTwitchBot/DiscordTwitchBot.csproj), and the host builder adds user secrets automatically in Development mode.

Use the .NET CLI to set local values:

```bash
dotnet user-secrets init --project src/DiscordTwitchBot
dotnet user-secrets set "Feature:LocalSecret" "local-dev-value" --project src/DiscordTwitchBot
```

The values are stored outside the repository and should not be committed. This project does not currently support `.env` files or other repository-local secret files.

## Deployment configuration

For non-local deployments, prefer environment variables in the target runtime environment. For example:

```bash
export Feature__LocalSecret="deployment-value"
```

The runtime precedence remains:

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets in Development
4. environment variables

## Secrets and logging guidance

Never log a configured secret value. When validating configuration, inspect the resolved value through `IConfiguration` and confirm the output does not contain the sentinel value. Missing-key validation should identify the key, not the associated value.

## Related references

- [Application architecture](../architecture/application-host.md)
- [Development setup](../development/local-run.md)
- [Project README](../../README.md)
