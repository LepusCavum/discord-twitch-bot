# Discord Twitch Bot

## Purpose

This repository implements a .NET 10 Discord bot that relays eligible Twitch chat activity to a single configured Discord channel while staying operational under common connectivity and runtime failures.

The project is intentionally scoped to a single Twitch channel and a single Discord server for v1.0. The work should remain aligned with the product requirements and architecture decisions in the planning documents.

## Business Requirements

Key features for v1.0:
- Connect to a single configured Twitch channel and receive/process Twitch chat messages and supported Twitch events.
- Process Twitch chat messages through filtering and formatting and reliably deliver eligible messages to a single configured Discord channel in the order received.
- Connect to configured Discord server and provide Discord-based interaction and runtime status information, including the /status command.
- Remain operational during recoverable Twitch, Discord, and relay failures by providing connection recovery, health monitoring, failure handling, diagnostic logging, and graceful shutdown.
- Support externally supplied configuration and secrets, allow relay behavior to be configured without source-code changes, and be packaged and documented so it can be deployed and operated as a self-contained Windows x64 application outside the development environment.

## Scope and Limits

This project should stay within the v1.0 scope and avoid adding features outside the current release definition.

- Out of scope for v1.0: see src\DiscordTwitchBot\docs\plans\future-work.md
- All proposed solutions should avoid implementing future-work items unless they are explicitly requested and approved.

## Technical Decisions

Follow the architecture decisions captured in src\DiscordTwitchBot\docs\plans\project-facts.md.

Key points:
- v1.0 is specifically a stable locally hosted bot for one Twitch streamer/channel and one Discord server, not a generalized SaaS-style bot.
- Work is to be done with TDD and xUnit.
- This is a C# / .NET 10 project.
- Project uses GitHub Actions for CI.
- External integrations communicate through application-owned interfaces and models; integration libraries should not leak across feature boundaries.
- Dependency injection is handled through the .NET Generic Host service container.
- Program.cs should remain thin and should not contain business logic.
- Configuration uses the .NET Options pattern.
- Use structured logging.
- Use async/await.
- TwitchLib will be used for chat and EventSub.
- Build and test are done from source; packaged release artifacts are separate from developer workflows.

## Architecture Guardrails

1. Keep Program.cs thin and composition-oriented only.
2. Keep application workflow logic in services rather than in startup composition.
3. Prefer application-owned abstractions over third-party library leakage.
4. Keep configuration externalized and option-driven.
5. Validate behavior through public contracts, interfaces, and application-owned models.
6. Avoid implementation-detail testing and avoid asserting on framework behavior.

## Coding Standards

1. Prefer supported, current dependencies compatible with the project target framework and existing constraints.
2. Do not introduce new frameworks, package churn, or architectural abstractions unless they are required to satisfy the current task.
3. Keep the solution simple: do not over-engineer, do not add speculative abstraction, and do not add defensive code that is not required by the current behavior.
4. Do not broaden scope without explicit approval.
5. Be concise. Keep documentation and README minimal. No emojis.
6. When investigating issues, identify the root cause before making a fix. Avoid speculative refactors and do not guess. Prove with evidence, then fix the root cause.
7. Production edits should be minimal and aligned with the existing architecture.
8. For any behavior change, write or update a failing xUnit test before implementing the fix.
9. A task is complete only when tests pass, behavior is documented where needed, and no architecture rule was violated.

## Required Workflow

Use this workflow for changes and bug fixes:

1. Review the relevant planning docs before changing behavior.
2. Confirm the requested work matches the v1.0 scope and architecture.
3. Identify the root cause and the exact behavior to change.
4. Write or update a failing xUnit test that captures the intended behavior.
5. Implement the minimal production fix.
6. Run the smallest relevant verification step.
7. Refactor only if it is necessary to preserve clarity and correctness.
8. Stop only when the evidence shows the fix is correct and aligned with the project rules.

## Approval Rules

- All changes require approval

## Testing Expectations

- Tests should validate observable behavior through public contracts and application-owned models.
- Tests should fail only when application behavior changes, not because of environmental conditions or framework implementation details.
- Use TDD: Red -> Green -> Refactor.
- Prefer the smallest relevant test set over broad suite execution.

## Working Documentation

All planning and execution documents for this project live under src\DiscordTwitchBot\docs\plans\.

Before making changes:
- Review the relevant documents in src\DiscordTwitchBot\docs\plans\.
- Confirm the work is consistent with the current v1.0 goals and architecture.
- Do not treat the planning docs as optional guidance when they are clearly relevant to the task.

## Completion Criteria

A task is considered complete only when all of the following are true:
- The requested behavior is implemented and matches the project requirements.
- The change stays within the v1.0 scope and architecture.
- A failing test or reproducer was established before the fix when behavior changed.
- The minimal relevant tests pass.
- The change does not introduce avoidable complexity or unsupported dependencies.
- Documentation remains aligned with the implementation.
