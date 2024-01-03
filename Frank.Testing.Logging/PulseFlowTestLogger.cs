using Frank.PulseFlow;
using Frank.PulseFlow.Logging;

using Microsoft.Extensions.Logging;

namespace Frank.Testing.Logging;

public class PulseFlowTestLogger(IConduit conduit) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) 
        => conduit.SendAsync(new LogPulse<TState>(logLevel, eventId, state, exception, formatter, "TestLogger")).GetAwaiter().GetResult();

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => new PulseFlowLoggerScope<TState>(state);
}