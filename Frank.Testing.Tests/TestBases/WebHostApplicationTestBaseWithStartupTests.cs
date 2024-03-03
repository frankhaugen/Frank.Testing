using FluentAssertions;

using Frank.Testing.Logging;
using Frank.Testing.TestBases;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class WebHostApplicationTestBaseWithStartupTests(ITestOutputHelper outputHelper) : WebApplicationTestBase(new InMemoryLoggerProvider(Options.Create(new LoggerFilterOptions() {MinLevel = LogLevel.Debug})))
{
    /// <inheritdoc />
    protected override Task SetupAsync(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IService, MyService>();
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task SetupApplicationAsync(WebApplication application)
    {
        application.UseRouting();
        application.MapControllers();
        application.MapGet("/test1", async httpContext =>
        {
            await httpContext.Response.WriteAsync("Test1 endpoint");
        });
        return Task.CompletedTask;
    }

    [Fact]  
    public async Task Test()
    {
        var service = GetServices.GetRequiredService<IService>();
        
        service.DoSomething();
        
        var myServiceLogger = GetServices.GetRequiredService<ILogger<MyService>>();
        var inMemoryLogger = myServiceLogger as InMemoryLogger<MyService>;
        inMemoryLogger?.GetLogEntries().Should().Contain(log => log.Message == "DoSomething");
    }
}

public interface IService
{
    void DoSomething();
}

public class MyService : IService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("DoSomething");
    }
}

public class CoolService : IService
{
    private readonly ILogger<CoolService> _logger;

    public CoolService(ILogger<CoolService> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("DoSomething else");
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSingleton<IService, MyService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
            endpoints.MapGet("/test2", async httpContext =>
            {
                await httpContext.Response.WriteAsync("Test2 endpoint");
            });
            
        });
    }
}