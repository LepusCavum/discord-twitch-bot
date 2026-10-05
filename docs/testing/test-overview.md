# Testing overview

The repository currently includes a focused xUnit test suite that verifies the Generic Host, dependency injection registration, logging, and startup validation behavior.

## Test project

The test project is located in [tests/DiscordTwitchBot.Tests](../../tests/DiscordTwitchBot.Tests).

It exercises the application using the real host builder and service registration flow, rather than relying solely on stubbed behavior, which keeps the tests aligned with the actual startup configuration and validation logic.

## Core test areas

The current suite covers:

- host creation and startup,
- host shutdown behavior,
- dependency injection wiring,
- `ApplicationOptions` validation,
- startup service lifecycle signaling,
- logging configuration in development and production,
- failure scenarios for unavailable dependencies and startup exceptions.

Representative files include:

- [tests/DiscordTwitchBot.Tests/Hosting/HostBuilderTests.cs](../../tests/DiscordTwitchBot.Tests/Hosting/HostBuilderTests.cs)
- [tests/DiscordTwitchBot.Tests/Dependency Injection/ServiceRegistrationTests.cs](../../tests/DiscordTwitchBot.Tests/Dependency%20Injection/ServiceRegistrationTests.cs)
- [tests/DiscordTwitchBot.Tests/Logging/LoggingExtensionsTests.cs](../../tests/DiscordTwitchBot.Tests/Logging/LoggingExtensionsTests.cs)
- [tests/DiscordTwitchBot.Tests/Configuration/ApplicationOptionsTests.cs](../../tests/DiscordTwitchBot.Tests/Configuration/ApplicationOptionsTests.cs)
- [tests/DiscordTwitchBot.Tests/Services/StartupServiceTests.cs](../../tests/DiscordTwitchBot.Tests/Services/StartupServiceTests.cs)

## Local verification commands

Run the full suite from the repository root:

```bash
dotnet test --nologo
```

This is the project's current validation command and matches the CI workflow behavior.

## Verification status

As of the current codebase, the suite passes successfully:

- 41 tests run
- 41 passed
- 0 failed

This confirms the current startup and validation architecture is behaving as expected under the repository's automated checks.

## Related references

- [Continuous integration](../ci/github-actions.md)
- [Project documentation index](../README.md)
