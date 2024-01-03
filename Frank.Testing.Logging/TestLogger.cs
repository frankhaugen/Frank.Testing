using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class TestLogger<T> : ILogger<T>
{
    public ITestOutputHelper OutputHelper { get; }
    public LogLevel LogLevel { get; }
    public string? CategoryName { get; }
    
    public TestLogger(ITestOutputHelper outputHelper, LogLevel logLevel, string? categoryName = null)
    {
        OutputHelper = outputHelper;
        LogLevel = logLevel;
        CategoryName = categoryName;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel < LogLevel)
            return;

        OutputHelper.WriteLine($"{logLevel}: {formatter(state, exception)}");
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return new TestLoggerScope<TState>(state);
    }
}