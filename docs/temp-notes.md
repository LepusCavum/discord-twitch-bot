Before writing any production code

Following TDD, I'd actually start with the two automated tests from the Story.

### Test 1 (Happy Path)

Required application services resolve successfully from the dependency injection container.

This proves DI is configured correctly.

Something similar to:
```
[Fact]
public void ServiceProvider_ShouldResolveApplicationServices()
{
    var services = new ServiceCollection();

    services.AddApplicationServices();

    using var provider = services.BuildServiceProvider();

    var startup = provider.GetRequiredService<IStartupService>();

    startup.Should().NotBeNull();
}
```

#### Test 2 (Failure Path)

The application fails when a required service registration is missing.
```
[Fact]
public void ServiceProvider_ShouldThrow_WhenRequiredServiceMissing()
{
    var services = new ServiceCollection();

    using var provider = services.BuildServiceProvider();

    Assert.Throws<InvalidOperationException>(() =>
        provider.GetRequiredService<IStartupService>());
}
```
Those two tests map directly to the story acceptance criteria.

What should we actually register?

Since this is still only v0.1, I'd intentionally keep the initial service graph tiny.

Program
    │
    ▼
IStartupService
    │
    ▼
StartupService

That's it.

Don't register Discord.

Don't register Twitch.

Don't register relay components.

Those come in later milestones.

Folder structure

I'd introduce a new folder now.
```
src/

    Configuration/

    DependencyInjection/
        ServiceCollectionExtensions.cs

    Services/
        IStartupService.cs
        StartupService.cs

    Program.cs
```
That folder will eventually become the single place where every project service is registered.

Later you'll simply have:
```
services
    .AddConfiguration()
    .AddLogging()
    .AddDiscord()
    .AddTwitch()
    .AddRelay()
    .AddRecovery();
```
instead of a 300-line Program.cs.

## Extension Method

I would create
```
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IStartupService, StartupService>();

        return services;
    }
}
```
Later this file will grow into
```
services
    .AddConfiguration()
    .AddLogging()
    .AddDiscord()
    .AddTwitch()
    .AddRelay()
    .AddRecovery();
```
without changing Program.cs.

## StartupService

Right now it should do almost nothing.
```
public interface IStartupService
{
    Task RunAsync();
}
public sealed class StartupService : IStartupService
{
    public Task RunAsync()
    {
        return Task.CompletedTask;
    }
}
```
Notice there is zero business logic.

Its only purpose today is proving DI works.

Later this class becomes the application's orchestrator.

Program.cs

Instead of manually constructing anything:
```
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();

using var host = builder.Build();

await host.Services
          .GetRequiredService<IStartupService>()
          .RunAsync();
```
Every future service now enters through DI.

## Lifetime choices

Even though there isn't much yet, I would start using lifetimes consistently.
```
Service	                Lifetime	Why
StartupService	        Singleton	One instance for the application's lifetime
Discord Client (future)	Singleton	Single gateway connection
Twitch Client (future)	Singleton	Single IRC/EventSub connection
Message Formatter	    Singleton	Stateless
Message Filter	        Singleton	Stateless
Relay Service	        Singleton	Coordinates long-lived services
Configuration	        Singleton	Managed by the host
Logger	                Built-in	Managed by .NET
```

Why not use IHostedService yet?

You'll probably see many .NET worker service examples using BackgroundService or IHostedService.

I would intentionally wait.

Reason:

Today your application has no background work.

If you introduce hosted services now, you're adding abstraction before it's needed.

When Discord is introduced in v0.2 Story 2, you'll naturally have something that runs continuously, and that will be the right time to convert the startup orchestration into a hosted service.

That keeps this story focused on its acceptance criteria while leaving a smooth migration path.




# Our plan for this project

Since you're new to C# testing, I'd like to take it one layer at a time.

We'll cover:

xUnit basics
- [Fact]
- [Theory]
- Assertions
- Test naming
FluentAssertions (later)
- Much more readable assertions.
Dependency Injection testing
- What should be tested.
- What should not be tested.
Mocking
- Moq (or a similar framework)
- Fake services
- Verifying interactions
Integration tests
- Testing the application startup.
- Testing configuration.
- Testing Discord/Twitch registration.
TDD workflow
- Red
- Green
- Refactor