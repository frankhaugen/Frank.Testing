using System.Net;

using Frank.Testing.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Frank.Testing.TestBases;

/// <summary>
/// The base class for web application tests that uses the <see cref="WebApplication"/> and <see cref="WebApplicationBuilder"/> to setup and run the web application for testing, and provides the <see cref="GetTestClient"/> to make requests to the web application using HttpClient
/// </summary>
public class WebApplicationTestBase : IAsyncDisposable
{
    private readonly WebApplicationBuilder _hostApplicationBuilder;
    private WebApplication? _application;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _initialized = false;
    private IServiceScope? _scope;

    /// <summary>
    /// Creates a new instance of <see cref="WebApplicationTestBase"/> with the specified logger provider and log level
    /// </summary>
    /// <param name="outputHelper"></param>
    /// <param name="logLevel"></param>
    /// <param name="loggerProvider"></param>
    protected WebApplicationTestBase(LogLevel logLevel = LogLevel.Error, ILoggerProvider? loggerProvider = null)
    {
        _hostApplicationBuilder = WebApplication.CreateBuilder();
        _hostApplicationBuilder.Logging.ClearProviders().AddDebug().AddProvider(TestContext.Current?.CreateTestLoggerProvider() ?? NullLoggerProvider.Instance).SetMinimumLevel(logLevel);
        
        if (loggerProvider != null)
        {
            _hostApplicationBuilder.Logging.AddProvider(loggerProvider);
        }
    }

    /// <summary>
    /// The services of the host application after it starts
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    protected IServiceProvider GetServices => (_initialized ? _scope?.ServiceProvider : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");

    /// <summary>
    /// Setup the web application before it starts
    /// </summary>
    /// <param name="builder">The web application builder to setup</param>
    protected virtual async Task SetupAsync(WebApplicationBuilder builder) => await Task.CompletedTask;

    /// <summary>
    /// Setup the host application
    /// </summary>
    /// <param name="application">The web application to setup</param>
    protected virtual async Task SetupApplicationAsync(WebApplication application) => await Task.CompletedTask;

    /// <summary>
    /// Returns the port to run the application on
    /// </summary>
    /// <returns></returns>
    protected virtual int GetPort() => Random.Shared.Next(5000, 6000 + 1);

    /// <summary>
    /// The test client to make requests to the web application using HttpClient
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    protected HttpClient GetTestClient => (_initialized ? _application?.CreateTestClient() : throw new InvalidOperationException("The host has not been initialized yet.")) ?? throw new InvalidOperationException("!!!");
    
    /// <summary>
    /// The endpoints of the web application after it starts. Useful for ensuring that the endpoints are registered correctly
    /// </summary>
    protected IEnumerable<Endpoint> GetEndpoints => GetServices.GetRequiredService<EndpointDataSource>().Endpoints;
    
    /// <summary>
    /// The routes of the web application after it starts. Useful for ensuring that the routes are registered correctly and for making requests to the web application using HttpClient
    /// </summary>
    protected IEnumerable<string> GetEndpointRoutes => GetEndpoints.Select(e => e).Cast<RouteEndpoint>().Select(e => e.RoutePattern.RawText)!;
    
    public async Task InitializeAsync()
    {
        await SetupAsync(_hostApplicationBuilder);
        _hostApplicationBuilder.WebHost.UseKestrel(kestrelOptions =>
        {
            kestrelOptions.Listen(IPAddress.Parse("127.0.0.1"), GetPort());
        });
        _application = _hostApplicationBuilder.Build();
        await SetupApplicationAsync(_application);
        await _application.StartAsync(_cancellationTokenSource.Token);
        _scope = _application.Services.CreateScope();
        _initialized = true;
    }

    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        await _application?.StopAsync()!;
        await _application.WaitForShutdownAsync();
    }
    
    public async Task StartAsync()
    {
        if (_application == null)
        {
            throw new InvalidOperationException("The host has not been initialized yet.");
        }
        await _application.StartAsync(_cancellationTokenSource.Token);
    }
    
    public async Task StopAsync()
    {
        if (_application == null)
        {
            throw new InvalidOperationException("The host has not been initialized yet.");
        }
        await _application.StopAsync(_cancellationTokenSource.Token);
    }
}