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

PR Merge Acceptance comment: `[Approved](link_to_approval_comment_in_Issue)`

## Testing tools

DOTNET_ENVIRONMENT=Development dotnet run
DOTNET_ENVIRONMENT=Production dotnet run

echo 'export MY_VAR="value"' >> ~/.bashrc
source ~/.bashrc