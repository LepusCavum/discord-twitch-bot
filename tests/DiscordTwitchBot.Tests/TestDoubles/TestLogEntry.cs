using Microsoft.Extensions.Logging;

public sealed class LogEntry
{
    public LogLevel Level { get; init; }

    public EventId EventId { get; init; }

    public string Message { get; init; } = string.Empty;

    public Exception? Exception { get; init; }
}