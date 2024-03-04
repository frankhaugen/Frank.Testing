using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class SimpleTestLoggerProvider(ITestOutputHelper outputHelper, IOptions<LoggerFilterOptions> options) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, SimpleTestLogger> _loggers = new();
    
    public SimpleTestLoggerProvider(ITestOutputHelper outputHelper): this(outputHelper, Options.Create<LoggerFilterOptions>(new LoggerFilterOptions() { MinLevel = LogLevel.Information }))
    {
    }
    
    /// <inheritdoc />
    public void Dispose() => _loggers.Clear();

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, new SimpleTestLogger(outputHelper, options.Value.MinLevel, categoryName));
}