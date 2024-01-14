using System.Collections.Concurrent;

using Frank.PulseFlow;

using Microsoft.Extensions.Logging;

namespace Frank.Testing.Logging;

public class TestLoggerProvider(IConduit conduit) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ILogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        if (_loggers.TryGetValue(categoryName, out var logger))
            return logger;

        var newLogger = new PulseFlowTestLogger(conduit, categoryName);
        return _loggers.GetOrAdd(categoryName, newLogger);
    }

    public void Dispose() => _loggers.Clear();
}