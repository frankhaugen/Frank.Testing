using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Frank.Testing.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddInMemoryLoggingProvider(this ILoggingBuilder builder, LogLevel logLevel = LogLevel.Debug)
    {
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        builder.AddProvider<InMemoryLoggerProvider>();
        return builder;
    }
    
    public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder) where T : class, ILoggerProvider
    {
        builder.Services.AddSingleton<ILoggerProvider, T>();
        return builder;
    }
    
    public static ILoggingBuilder AddSimpleTestLogger(this ILoggingBuilder builder, LogLevel logLevel = LogLevel.Debug)
    {
        builder.Services.AddSingleton<ILoggerProvider, SimpleTestLoggerProvider>();
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = logLevel;
        });
        return builder;
    }
}