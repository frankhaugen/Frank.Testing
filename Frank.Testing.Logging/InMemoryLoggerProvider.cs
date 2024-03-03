using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Frank.Testing.Logging;

public class InMemoryLoggerProvider(IOptions<LoggerFilterOptions> options) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers = new();

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, new InMemoryLogger(options, categoryName));

    /// <inheritdoc />
    public void Dispose() => _loggers.Clear();
}