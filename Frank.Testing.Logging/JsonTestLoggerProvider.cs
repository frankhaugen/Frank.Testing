using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public class JsonTestLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly LogLevel _logLevel;

    public JsonTestLoggerProvider(ITestOutputHelper outputHelper, LogLevel logLevel)
    {
        _outputHelper = outputHelper;
        _logLevel = logLevel;
    }

    public ILogger CreateLogger(string categoryName) => new JsonTestLogger(_outputHelper, _logLevel, categoryName);

    public void Dispose()
    {
    }
}