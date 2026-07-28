## User Story

As a developer,
I want the application host lifecycle to be configured correctly
So that application services are initialized, managed, and shut down through the .NET Generic Host.

---

## Acceptance Criteria

1. [ ]Program.cs contains only application composition logic and does not contain business workflow.
2. [ ]The Generic Host is configured during application startup.
3. [ ]The Generic Host is capable of managing hosted services when they are introduced. 
4. [ ]Required application services are resolved successfully from the Generic Host service provider. 
5. [ ]StartupService is resolved through dependency injection.
6. [ ]StartupService executes after the host is built.
7. [ ]The application lifetime is managed by the Generic Host.
8. [ ]Shutdown requests propagate a cancellation token to StartupService.
9. [ ]The application shuts down gracefully without unhandled exceptions.


---

## Notes

This story establishes the application's hosting foundation. The Generic Host is responsible for application lifetime, dependency injection, and graceful shutdown, while Program.cs remains responsible only for application composition and startup configuration.

---

## Tasks

1. [x]Configure the .NET Generic Host.
2. [x]Configure service registration through extension methods.
3. [x]Create the IStartupService abstraction.
4. [x]Implement StartupService.
5. [x]Register StartupService with dependency injection.
6. [x]Resolve StartupService through the Generic Host.
7. [x]Execute the startup sequence through StartupService.
8. [ ]Configure host service provider validation for development. 
9. [x]Execute the startup sequence through the host lifecycle.
10. [x]Implement graceful shutdown token propagation.

Verify application startup and shutdown behavior.

---

## Test Plan

### Automated Tests

1. [x]A configured Generic Host can be created with all required application registrations. 
2. [x]The Generic Host builds successfully. [CreateHost_BuildsSuccessfully]
3. [x]Required services can be resolved through the Generic Host. [Host_ResolvesStartupService]
4. [x]StartupService resolves through dependency injection. [StartupService_ShouldResolveSuccessfully]
5. [x]StartupService executes successfully after host initialization. [StartAsync_CompletesSuccessfully]
6. [x]StartupService receives the application shutdown cancellation token. [StartupService_RecievesShutdownCancellationToken]
7. [x]StartupService receives the application shutdown cancellation token. [StartupService_RecievesShutdownCancellationToken]
8. [x]The Generic Host starts and stops successfully. [Host_StartsSuccessfully, Host_StopsSuccessfully]
8. [x]Application shutdown completes without unhandled exceptions. [Host_StopsSuccessfully]

### Manual Verification

1. [ ]Build the application.
2. [ ]Start the application.
3. [ ]Verify startup completes successfully.
4. [ ]Verify startup logs are generated.
5. [ ]Stop the application.
6. [ ]Verify shutdown completes cleanly without unhandled exceptions.