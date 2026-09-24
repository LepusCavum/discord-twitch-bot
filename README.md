# discord-twitch-bot
Discord bot that monitors twitch chat messages to perform various actions in a discord server

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
