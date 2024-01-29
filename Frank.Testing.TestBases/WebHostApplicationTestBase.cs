using Frank.Testing.Logging;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using Xunit;
using Xunit.Abstractions;

namespace Frank.Testing.TestBases;

public abstract class WebHostApplicationTestBase : IAsyncLifetime
{
    private readonly IWebHostBuilder _hostApplicationBuilder;
    private IWebHost? _host;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _initialized = false;

    protected WebHostApplicationTestBase(ITestOutputHelper outputHelper, LogLevel logLevel = LogLevel.Information)
    {
        _hostApplicationBuilder = WebHost.CreateDefaultBuilder();
        _hostApplicationBuilder.ConfigureLogging(logging => logging.AddSimpleTestLoggingProvider(outputHelper, logLevel));
    }

    public IServiceProvider Services => (_initialized ? _host?.Services : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");

    protected virtual async Task SetupAsync(IWebHostBuilder builder) => await Task.CompletedTask;
    
    protected HttpClient TestClient => (_initialized ? _host?.CreateTestClient() : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");
    protected IEnumerable<string> GetServerEndpoints() => (_initialized ? _host?.GetServerEndpoints() : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");
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