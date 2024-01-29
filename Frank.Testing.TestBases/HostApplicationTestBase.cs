using Frank.Testing.Logging;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit;
using Xunit.Abstractions;

namespace Frank.Testing.TestBases;

public abstract class HostApplicationTestBase : IAsyncLifetime
{
    private readonly HostApplicationBuilder _hostApplicationBuilder;
    private IHost? _host;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _initialized = false;

    protected HostApplicationTestBase(ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Information)
    {
        _hostApplicationBuilder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
        _hostApplicationBuilder.Logging.AddSimpleTestLoggingProvider(outputHelper, logLevel);
    }

    public IServiceProvider Services => (_initialized ? _host?.Services : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");

    protected virtual async Task SetupAsync(HostApplicationBuilder builder) => await Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await SetupAsync(_hostApplicationBuilder);
        _host = _hostApplicationBuilder.Build();
        await _host.StartAsync(_cancellationTokenSource.Token);
        _initialized = true;
    }

    public async Task DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        await _host?.StopAsync()!;
        await _host.WaitForShutdownAsync();
        _host.Dispose();
    }
}