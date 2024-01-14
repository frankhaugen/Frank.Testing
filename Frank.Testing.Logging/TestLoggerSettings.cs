using Microsoft.Extensions.Logging;

namespace Frank.Testing.Logging;

public class TestLoggerSettings
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}