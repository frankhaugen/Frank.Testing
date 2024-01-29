using Frank.PulseFlow.Logging;
using Frank.Reflection;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class SimpleTestLogger<T>(ITestOutputHelper outputHelper, LogLevel logLevel) : SimpleTestLogger(outputHelper, logLevel, typeof(T).GetDisplayName()), ILogger<T>;

public class SimpleTestLogger(ITestOutputHelper outputHelper, LogLevel level, string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull 
        => new PulseFlowLoggerScope<TState>(state);

    public bool IsEnabled(LogLevel logLevel) => logLevel >= level;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel < level)
            return;

        outputHelper.WriteLine(new LogPulse(logLevel, eventId, exception, categoryName, formatter.Invoke(state, exception), state as IReadOnlyList<KeyValuePair<string, object?>>).ToString());
    }
}