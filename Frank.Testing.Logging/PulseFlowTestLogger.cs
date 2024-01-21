using Frank.PulseFlow;
using Frank.PulseFlow.Logging;
using Frank.Reflection;

using Microsoft.Extensions.Logging;

namespace Frank.Testing.Logging;

public class PulseFlowTestLogger(IConduit conduit, string categoryName, TestLoggerSettings settings) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) 
        => conduit.SendAsync(new LogPulse(logLevel, eventId, exception, categoryName, formatter(state, exception))).GetAwaiter().GetResult();

    public bool IsEnabled(LogLevel logLevel) => logLevel >= settings.LogLevel;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => new PulseFlowLoggerScope<TState>(state);
}

public class PulseFlowTestLogger<T>(IConduit conduit, TestLoggerSettings settings) : PulseFlowTestLogger(conduit, typeof(T).GetDisplayName(), settings), ILogger<T>;