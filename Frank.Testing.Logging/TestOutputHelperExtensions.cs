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
    /// <param name="logLevel"></param>
    /// <returns>An instance of <see cref="ILogger{T}"/> that can be used for logging tests.</returns>
    public static ILogger<T> CreateTestLogger<T>(this ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
        => new SimpleTestLogger<T>(outputHelper, logLevel);
}
