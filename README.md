# discord-twitch-bot
Discord bot that monitors twitch chat messages to perform various actions in a discord server

## Configuration and secrets

This application reads configuration through the .NET Generic Host. The supported local secret workflow is .NET User Secrets for Development. Deployment credentials should be supplied through environment variables in the deployment environment, not committed to the repository.

### Local development with User Secrets

1. Set the environment to Development.
2. Run the app once to ensure the project has a User Secrets ID configured in the project file.
3. Use the .NET CLI to initialize and set a local value:

```bash
dotnet user-secrets init --project src/DiscordTwitchBot
dotnet user-secrets set "Feature:LocalSecret" "local-dev-value" --project src/DiscordTwitchBot
```

The values are stored outside the repository in the local user secrets store. This app does not support `.env` files or other repository-local secret files.

### Deployment configuration

For deployment, set values through environment variables in the target runtime environment. For example:

```bash
export Feature__LocalSecret="deployment-value"
```

The runtime precedence is:

1. appsettings.json
2. appsettings.{Environment}.json
3. User Secrets in Development
4. environment variables

The application does not currently read command-line configuration values, so command-line configuration is intentionally not documented as a supported secret source.

### Secrets and logging

Never log a configured secret value. When verifying configuration, inspect the resolved value through `IConfiguration` and confirm log output does not contain the sentinel value. Missing-key validation should identify the configuration key only, not the associated value.

## Conventions:
Branches: `feat/v<Milestone>.<Issue#>-<short-title>` 

Issues: `Issue <Issue#>: <message>` 

PR Title: `PR <Issue#>: <Issue Title>` 

Code Review Format:
```
Acceptance Criteria:
1. [ ] AC1 from Issue
2. [ ] AC2 from Issue
3. [ ] AC3 from Issue
etc...

Manual Verification:
1. [ ] Step 1 from Issue
2. [ ] Step 2 from Issue
3. [ ] Step 3 from Issue
etc...

Notes
<Any additional comments.>

Criteria met/failed. PR can/can NOT be merged.
```

## Local run commands

This project uses the .NET Generic Host, so the environment is controlled with `DOTNET_ENVIRONMENT`.

DOTNET_ENVIRONMENT=Development dotnet run --project src/DiscordTwitchBot
DOTNET_ENVIRONMENT=Production dotnet run --project src/DiscordTwitchBot

echo 'export MY_VAR="value"' >> ~/.bashrc
source ~/.bashrc
