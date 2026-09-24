using Microsoft.Extensions.Logging;

public sealed class TestLogger<T> : ILogger<T>, ILoggerProvider
{
    private readonly List<TestLogEntry> _entries = [];
    public IReadOnlyList<TestLogEntry> Entries => _entries;

    // Registering the logger as a provider lets host-created ILogger instances use this same entry collection.
    public ILogger CreateLogger(string categoryName)
    {
        return this;
    }

    public void Dispose()
    {
    }

    // Tests do not need scope state, but ILogger still requires a disposable scope object.
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _entries.Add(new TestLogEntry
        {
            Level = logLevel,
            EventId = eventId,
            Message = formatter(state, exception),
            Exception = exception
        });
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}