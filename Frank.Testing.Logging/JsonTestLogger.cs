using System.Text.Json;

using Frank.PulseFlow.Logging;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class JsonTestLogger : ILogger
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly LogLevel _logLevel;
    private readonly string _categoryName;
    
    public JsonTestLogger(ITestOutputHelper outputHelper, LogLevel logLevel, string categoryName)
    {
        _outputHelper = outputHelper;
        _logLevel = logLevel;
        _categoryName = categoryName;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        
        var json = JsonFormatter.Format(state, exception);
        
        JsonDocument document = JsonDocument.Parse(formatter.Invoke(state, exception));
        
        _outputHelper.WriteLine(new LogPulse(logLevel, eventId, exception, _categoryName, formatter.Invoke(state, exception), state as IReadOnlyList<KeyValuePair<string, object?>>).ToString());
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return _logLevel <= logLevel;
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}