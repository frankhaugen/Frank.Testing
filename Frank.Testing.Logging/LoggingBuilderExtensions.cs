using Frank.PulseFlow;
using Frank.PulseFlow.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds test logging to the ILoggingBuilder.
    /// </summary>
    /// <param name="builder">The ILoggingBuilder to add the test logging to.</param>
    /// <param name="outputHelper">The ITestOutputHelper to redirect the logging output to.</param>
    /// <param name="logLevel">The log level to use for the test logging. Default is LogLevel.Debug.</param>
    /// <returns>The modified ILoggingBuilder with the test logging added.</returns>
    public static ILoggingBuilder AddPulseFlowTestLoggingProvider(this ILoggingBuilder builder, ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        builder.AddPulseFlow();
        builder.Services.AddSingleton(outputHelper);
        builder.Services.AddSingleton<IFlow, TestLoggingOutputFlow>();
        return builder;
    }
}