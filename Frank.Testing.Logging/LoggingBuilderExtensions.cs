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
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        builder.Services.AddPulseFlow(flowBuilder => flowBuilder.AddFlow<TestLoggingOutputFlow>());
        return builder;
    }
    
    public static ILoggingBuilder AddSimpleTestLoggingProvider(this ILoggingBuilder builder, ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        builder.Services.AddSingleton(outputHelper);
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        builder.AddProvider<SimpleTestLoggerProvider>();
        return builder;
    }

    public static ILoggingBuilder AddInMemoryLoggingProvider(this ILoggingBuilder builder, LogLevel logLevel = LogLevel.Debug)
    {
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        builder.AddProvider<InMemoryLoggerProvider>();
        return builder;
    }
    
    public static ILoggingBuilder AddJsonTestLoggingProvider(this ILoggingBuilder builder, ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        builder.Services.AddSingleton(outputHelper);
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        builder.AddProvider<JsonTestLoggerProvider>();
        return builder;
    }
    
    public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder) where T : class, ILoggerProvider
    {
        builder.Services.AddSingleton<ILoggerProvider, T>();
        return builder;
    }
}