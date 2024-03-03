using Frank.Reflection;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Frank.Testing.Logging;

public class InMemoryLogger(IOptions<LoggerFilterOptions> options, string category) : ILogger
{
    private readonly List<InMemoryLogEntry> _logEntries = new();
    
    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) 
        => _logEntries.Add(new InMemoryLogEntry(logLevel,  eventId, exception, category, formatter(state, exception), state as IReadOnlyList<KeyValuePair<string, object?>>));

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => options.Value.Rules.Any(rule => rule.ProviderName == "InMemoryLogger" && rule.LogLevel <= logLevel);

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => new InMemoryLoggerScope<TState>(state);
    
    public IReadOnlyList<InMemoryLogEntry> GetLogEntries() => _logEntries;
}

public class InMemoryLogger<T> : InMemoryLogger, ILogger<T>
{
    public InMemoryLogger(IOptions<LoggerFilterOptions> options) : base(options, typeof(T).GetFullFriendlyName()) { }
}