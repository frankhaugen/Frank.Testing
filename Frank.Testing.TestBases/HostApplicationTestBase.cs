using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit;

namespace Frank.Testing.TestBases;

/// <summary>
/// Base class for tests that require a host application to be started and stopped for testing like integration tests using HostedServices or background services in the host application
/// </summary>
public abstract class HostApplicationTestBase : IAsyncLifetime
{
    private readonly HostApplicationBuilder _hostApplicationBuilder;
    private IHost? _host;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _initialized = false;
    private IServiceScope? _scope;

    /// <summary>
    /// Creates a new instance of <see cref="HostApplicationTestBase"/> with the specified logger provider and log level
    /// </summary>
    /// <param name="loggerProvider"></param>
    /// <param name="logLevel"></param>
    protected HostApplicationTestBase(ILoggerProvider loggerProvider, LogLevel logLevel = LogLevel.Error)
    {
        _hostApplicationBuilder = Host.CreateApplicationBuilder();
        _hostApplicationBuilder.Logging.AddDebug().AddProvider(loggerProvider).SetMinimumLevel(logLevel);
    }

    /// <summary>
    /// The services of the host application after it starts
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    protected IServiceProvider GetServices => (_initialized ? _scope?.ServiceProvider : throw new InvalidOperationException("Not initialized yet.")) ?? throw new InvalidOperationException("Unreachable situation.");

    /// <summary>
    /// Setup the host application before it starts
    /// </summary>
    /// <param name="builder"></param>
    protected virtual async Task SetupAsync(HostApplicationBuilder builder) => await Task.CompletedTask;

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await SetupAsync(_hostApplicationBuilder);
        _host = _hostApplicationBuilder.Build();
        await _host.StartAsync(_cancellationTokenSource.Token);
        _scope = _host.Services.CreateScope();
        _initialized = true;
    }
    
    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        await _host?.StopAsync()!;
        await _host.WaitForShutdownAsync();
        _host.Dispose();
    }
}