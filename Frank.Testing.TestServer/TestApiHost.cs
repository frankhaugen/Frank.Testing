using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Frank.Testing.TestServer;


/// <summary>
/// Spin-up an in-process ASP.NET Core host with a pre-wired <see cref="HttpClient"/>
/// for integration/in-memory testing.  
/// Exposes a fluent <see cref="Builder"/> so you can say:
///
/// <code>
/// await TestApiHost.Create()
///     .With(HttpMethod.Get, "/ping", _ => Results.Text("pong"))
///     .WithMiddleware(ctx =>
///     {
///         ctx.Response.Headers.Add("x-test", "1");
///         return Task.CompletedTask;
///     })
///     .Build(new Uri("http://127.0.0.1:0"))
///     .ExecuteAsync(async client =>
///     {
///         var txt = await client.GetStringAsync("/ping");
///         Console.WriteLine(txt);
///     });
/// </code>
/// </summary>
public sealed class TestApiHost : IAsyncDisposable
{
    
    /// <summary>Underlying ASP.NET Core application. Use this for advanced tweaks or DI access.</summary>
    public WebApplication App { get; }

    /// <summary>HTTP client already pointing at <see cref="App"/>.</summary>
    public HttpClient Client { get; }
    
    private TestApiHost(WebApplication app, HttpClient client) =>
        (App, Client) = (app, client);

    #region Lifecycle helpers
    /// <summary>
    /// Start the host, invoke <paramref name="work"/>, and *always* stop, even on exception.
    /// </summary>
    public async Task ExecuteAsync(
        Func<HttpClient, Task> work,
        CancellationToken      ct = default)
    {
        await App.StartAsync(ct);
        try     { await work(Client); }
        finally { await App.StopAsync(ct); }
    }

    /// <summary>
    /// Start the host, invoke <paramref name="work"/>, and *always* stop, even on exception. Provides both HttpClient and IServiceProvider.
    /// </summary>
    public async Task ExecuteAsync(
        Func<HttpClient, IServiceProvider, Task> work,
        CancellationToken ct = default)
    {
        await App.StartAsync(ct);
        try     { await work(Client, App.Services); }
        finally { await App.StopAsync(ct); }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync() => await App.StopAsync();
    #endregion

    #region Builder inner-class
    /// <summary>
    /// Fluent builder for <see cref="TestApiHost"/>. Create via <see cref="Create"/>.
    /// </summary>
    public sealed class Builder
    {
        private int? _port;
        
        private readonly List<(HttpMethod Verb, string Path, Func<HttpContext, Task> Handler)>
            _routes = new();
        private readonly List<Func<WebApplication, WebApplication>> _middleware = new();

        /// <summary>
        /// Register a route stub.  
        /// When <paramref name="verb"/> hits <paramref name="path"/>, <paramref name="handler"/> runs
        /// and its <see cref="IResult"/> is written back to the caller.
        /// </summary>
        public Builder With(
            HttpMethod                               verb,
            string                                   path,
            Func<HttpContext, Task>                  handler)
        {
            _routes.Add((verb, path, handler));
            return this;
        }

        /// <summary>
        /// Register middleware with the minimal signature <c>Func&lt;HttpContext,Task&gt;</c>.  
        /// Your delegate runs, then the rest of the pipeline continues automatically.
        /// </summary>
        public Builder WithMiddleware(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _middleware.Add(app => (WebApplication)app.Use(middleware));
            return this;
        }

        /// <summary>
        /// Build a <see cref="TestApiHost"/> bound to <paramref name="baseAddress"/>.
        /// Optional <paramref name="configure"/> lets you tweak DI/logging before the app is built.
        /// </summary>
        public TestApiHost Build(
            Uri                               baseAddress,
            Action<WebApplicationBuilder>?    configure = null)
        {
            var b = WebApplication.CreateBuilder();
            configure?.Invoke(b);
            b.WebHost.UseUrls(baseAddress.ToString());

            var app = b.Build();

            // add middleware in registration order
            foreach (var use in _middleware) use(app);

            // map routes
            foreach (var (verb, path, del) in _routes)
                app.MapMethods(path, [ verb.Method ], del);
            
            var uriBuilder = new UriBuilder();
            
            uriBuilder.Scheme = baseAddress.Scheme;
            uriBuilder.Host = baseAddress.Host;
            uriBuilder.Port = _port ??= GetRandomUnusedPort();
            
            var client = new HttpClient { BaseAddress = uriBuilder.Uri };
            
            app.Urls.Add(uriBuilder.Uri.ToString());
            
            return new(app, client);
        }

        /// <summary>Make a random unused port for the server to bind to.</summary>
        private static int GetRandomUnusedPort()
        {
            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
    #endregion

    /// <summary>Entry point for the fluent builder.</summary>
    public static Builder Create() => new();
} 