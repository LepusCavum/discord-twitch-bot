# Project Facts

Status: Active
Last reviewed: 2026-09-21

This document captures the current architectural and delivery decisions for the project. It is intentionally concise and intended to guide implementation decisions without expanding the v1.0 scope.

## 1. Product scope

- v1.0 is a stable local bot for one Twitch streamer/channel and one Discord server.
- The project is not a generalized SaaS-style bot platform.
- Scope is intentionally limited to one configured relay flow between Twitch and Discord.
- Future capability should be treated as follow-on work unless explicitly approved.

## 2. Technology baseline

- This is a C# / .NET 10 application.
- The application uses the .NET Generic Host and dependency injection container.
- The project follows a test-first workflow using xUnit.
- The solution uses GitHub Actions for CI.
- Development is source-based; release artifacts are produced as packaged build outputs for deployment.

## 3. Architecture rules

### 3.1 Host and startup

- Program.cs remains a thin application entry point.
- Host construction and dependency registration are performed by BotHost.Create(), which configures the Generic Host and registers application services.
- Program.cs does not contain business logic, feature orchestration, or workflow behavior; it primarily starts the host and handles top-level startup exceptions.
- StartupService is the current startup coordinator and hosted service; it logs startup and shutdown lifecycle events and participates in the host lifetime.
- The Generic Host owns application lifetime, shutdown, and hosted-service lifecycle management.
- Current composition pattern: BotHost.Create() -> build host -> Program.cs runs host -> StartupService participates in host startup/shutdown lifecycle.

### 3.2 Configuration

- Configuration follows the .NET Options pattern.
- Each feature owns its own configuration section and its own options model.
- Feature registration is handled by feature-specific extension methods, for example:
  - services.AddDiscord(configuration)
- Each service extension method should be responsible for:
  - binding the relevant configuration section
  - registering options with DI
  - validating startup requirements
  - registering feature services

### 3.3 External integrations

- External integrations are isolated behind application-owned interfaces and models.
- Third-party libraries and platform contracts should not leak across feature boundaries.
- Application behavior is defined by the app's own contracts, not by framework or library internals.
- The bot may use TwitchLib for Twitch chat and EventSub integration.

### 3.4 Logging and execution model

- Structured logging is required.
- All exceptions must be logged using the custom logging model.
- The application uses async/await for asynchronous work.
- The bot should avoid blocking or synchronous work in request-processing and runtime lifecycle paths.
- Logging should be safe for production and should never emit secrets or sensitive values.

## 4. Implementation standards

- Prefer simple, explicit implementations over speculative abstraction.
- Keep the solution minimal and aligned with v1.0 scope.
- Avoid adding new frameworks or dependency churn without a clear requirement.
- Do not broaden the design without explicit approval.
- Prefer behavior driven by public contracts and observable application results.

## 5. Testing standards

- Automated testing is already established and should be extended as functionality grows.
- Tests are written before production code when behavior changes.
- The preferred method is Red -> Green -> Refactor.
- Tests should validate observable behavior through public contracts, interfaces, and application-owned models.
- Tests should not validate .NET runtime behavior, third-party library internals, or live external service behavior.
- Tests should fail only when application behavior changes, not because of environment drift or framework implementation changes.

## 6. Operational workflow

### Development workflow

- Source -> Build -> Test

### Release workflow

- Source -> CI -> Publish artifact -> Deploy -> Run

## 7. Decision summary

1. v1.0 is intentionally narrow and local-first.
2. The project uses xUnit and TDD.
3. The application is a .NET 10 solution using the Generic Host.
4. Program.cs stays thin; startup orchestration is handled by host composition and startup services.
5. Configuration is feature-owned and uses the Options pattern.
6. External libraries are wrapped behind application-owned contracts.
7. Structured logging and async/await are required.
8. TwitchLib is the expected integration library for chat/EventSub features.
9. CI and packaged releases are part of the delivery model.
10. Tests validate application behavior rather than framework or provider internals.

## 8. Scope guardrails

The following are explicitly out of scope unless approved:

- generalized multi-streamer or multi-server support
- broad platform abstraction beyond the app's v1.0 needs
- cross-cutting redesigns that are not required by the current behavior
- speculative features that do not contribute to the v1.0 bot workflow

This document should remain stable and should only change when the project scope, architecture, or delivery rules change materially.
