using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Frank.Testing.Logging;

public class SimpleTestLoggerProvider(TestContext outputHelper, IOptions<LoggerFilterOptions> options) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, SimpleTestLogger> _loggers = new();
    
    public SimpleTestLoggerProvider(TestContext outputHelper): this(outputHelper, Options.Create<LoggerFilterOptions>(new LoggerFilterOptions() { MinLevel = LogLevel.Information }))
    {
    }
    
    /// <inheritdoc />
    public void Dispose() => _loggers.Clear();

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, new SimpleTestLogger(outputHelper, options.Value.MinLevel, categoryName));
}