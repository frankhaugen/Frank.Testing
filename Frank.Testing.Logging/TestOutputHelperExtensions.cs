using Frank.PulseFlow.Logging;
using Frank.Reflection;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public static class TestOutputHelperExtensions
{
    /// <summary>
    /// Creates a test logger instance with the specified log category name.
    /// </summary>
    /// <typeparam name="T">The type of the class to which the created logger will be associated.</typeparam>
    /// <param name="outputHelper">The ITestOutputHelper instance used for logging.</param>
    /// <param name="categoryName">The name of the log category. (optional)</param>
    /// <param name="logLevel"></param>
    /// <returns>An instance of <see cref="ILogger{T}"/> that can be used for logging tests.</returns>
    public static ILogger<T> CreateTestLogger<T>(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
        => new TestLogger<T>(outputHelper, logLevel, typeof(T).GetDisplayName());

    /// <summary>
    /// Creates a test logger factory using the specified <paramref name="outputHelper"/>.
    /// </summary>
    /// <param name="outputHelper">The test output helper.</param>
    /// <param name="logLevel"></param>
    /// <returns>A new instance of <see cref="ILoggerFactory"/> for testing purposes.</returns>
    public static ILoggerFactory CreateTestLoggerFactory(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        var factory = new LoggerFactory();
        // factory.AddProvider(new PulseFlowLoggerProvider());
        return factory;
    }
}