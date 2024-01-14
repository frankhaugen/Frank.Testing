using Frank.Reflection;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class SimpleTestLogger<T> : SimpleTestLogger, ILogger<T>
{
    public SimpleTestLogger(ITestOutputHelper outputHelper, LogLevel logLevel) : base(outputHelper, logLevel, typeof(T).GetDisplayName())
    {
    }
}

public class SimpleTestLogger : ILogger
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly LogLevel _logLevel;
    private string _categoryName;
    
    public SimpleTestLogger(ITestOutputHelper outputHelper, LogLevel logLevel, string categoryName)
    {
        _outputHelper = outputHelper;
        _logLevel = logLevel;
        _categoryName = categoryName;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull 
        => new TestLoggerScope<TState>(state);

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _logLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel < _logLevel)
            return;

        _outputHelper.WriteLine($"[{logLevel}]: {formatter(state, exception)}");
    }
}