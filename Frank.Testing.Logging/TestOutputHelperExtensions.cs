using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public static class TestOutputHelperExtensions
{
    /// <summary>
    /// Creates a test logger instance with the specified log category name.
    /// </summary>
    /// <typeparam name="T">The type of the class to which the created logger will be associated.</typeparam>
    /// <param name="outputHelper">The ITestOutputHelper instance used for logging.</param>
    /// <param name="logLevel"></param>
    /// <returns>An instance of <see cref="ILogger{T}"/> that can be used for logging tests.</returns>
    public static ILogger<T> CreateTestLogger<T>(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
        => new SimpleTestLogger<T>(outputHelper, logLevel);

    /// <summary>
    /// Creates a test logger instance with the specified log category name.
    /// </summary>
    /// <typeparam name="T">The type of the class to which the created logger will be associated.</typeparam>
    /// <param name="outputHelper">The ITestOutputHelper instance used for logging.</param>
    /// <param name="logLevel">The log level to be used for the logger (default: LogLevel.Debug).</param>
    /// <returns>An instance of <see cref="ILogger{T}"/> that can be used for logging tests.</returns>
    public static ILogger CreateTestLogger(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug, string categoryName = "Test")
        => new SimpleTestLogger(outputHelper, logLevel, categoryName);

    /// <summary>
    /// Creates a test logger provider instance with the specified log level.
    /// </summary>
    /// <param name="outputHelper">The ITestOutputHelper instance used for logging.</param>
    /// <param name="logLevel">The log level to be used for the logger provider (default: LogLevel.Debug).</param>
    /// <returns>An instance of ILoggerProvider that can be used for logging tests.</returns>
    public static ILoggerProvider CreateTestLoggerProvider(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
        => new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions()
        {
            MinLevel = logLevel
        }));

    /// <summary>
    /// Creates a test logger factory instance with the specified log level.
    /// </summary>
    /// <param name="outputHelper">The ITestOutputHelper instance used for logging.</param>
    /// <param name="logLevel">The log level to be used for the logger factory (default: LogLevel.Debug).</param>
    /// <returns>An instance of ILoggerFactory that can be used for creating test loggers.</returns>
    public static ILoggerFactory CreateTestLoggerFactory(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(outputHelper.CreateTestLoggerProvider(logLevel));
        });
        return loggerFactory;
    }
}
