# Discord Twitch Bot

## Mission
Build a .NET 10 Discord bot that relays eligible Twitch chat activity to one configured Discord channel while handling recoverable connectivity issues and runtime failures safely.

## Scope
- v1.0 is limited to one Twitch channel, one Discord server, and one relay flow.
- Follow the planning constraints in src\DiscordTwitchBot\docs\plans\project-facts.md.
- Do not add items from src\DiscordTwitchBot\docs\plans\future-work.md unless explicitly requested and approved.

## Architecture Rules
- Use the .NET Generic Host and DI container.
- Keep Program.cs thin; no business logic there.
- Use the Options pattern for configuration.
- Keep external integrations behind application-owned abstractions and models.
- Use structured logging and async/await.
- Prefer behavior through public contracts and observable application logic, not framework internals.

## Coding Rules
1. Prefer supported, current dependencies compatible with the project target framework.
2. Avoid new frameworks, dependency churn, and speculative abstraction.
3. Do not broaden scope without explicit approval.
4. Keep the implementation minimal and architecture-aligned.
5. No unnecessary defensive code or over-engineering.
6. Be concise; keep docs and README minimal. No emojis.
7. Identify the root cause before changing code. No guessing. No speculative refactors.
8. For any behavior change, write or update a failing xUnit test before the fix.

## Required Workflow
1. Review relevant planning docs before editing behavior.
2. Confirm the work matches v1.0 scope and architecture.
3. Identify root cause and exact behavior to change.
4. Write or update a failing xUnit test.
5. Implement the smallest valid fix.
6. Run the smallest relevant verification.
7. Refactor only if it preserves clarity and correctness.

## Approval Rules
- Small, clearly scoped updates may proceed without additional approval when they remain within the current architecture and v1.0 scope.
- Design changes, new abstractions, new dependencies, cross-cutting refactors, or scope expansion require explicit approval first.

## Completion Criteria
A task is complete only when:
- the behavior matches the project requirements,
- the change stays within v1.0 scope,
- the relevant tests pass,
- a failing test or reproducer was created before the fix when behavior changed,
- no architecture rule was violated.

## Working Documentation
Review the project docs in src\DiscordTwitchBot\docs\plans\ before proceeding on anything that affects behavior, architecture, or release scope.
