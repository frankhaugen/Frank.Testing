using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds test logging to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the test logging to.</param>
    /// <param name="outputHelper">The ITestOutputHelper to redirect the logging output to.</param>
    /// <param name="logLevel">The log level to use for the test logging. Default is LogLevel.Debug.</param>
    /// <returns>The modified IServiceCollection with the test logging added.</returns>
    public static IServiceCollection AddTestLogging(this IServiceCollection services, ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Debug)
    {
        services.AddLogging(builder => builder.AddPulseFlowTestLoggingProvider());
        return services;
    }
}