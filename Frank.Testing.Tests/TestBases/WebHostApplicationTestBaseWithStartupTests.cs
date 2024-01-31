using System.Net;

using FluentAssertions;

using Frank.Testing.TestBases;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class WebHostApplicationTestBaseWithStartupTests(ITestOutputHelper outputHelper) : WebHostApplicationTestBase(outputHelper)
{
    protected override Task SetupAsync(IWebHostBuilder builder)
    {
        builder.UseStartup<Startup>();
        builder.ConfigureServices(services =>
        {
            services.Replace<IService, CoolService>();
        });
        return Task.CompletedTask;
    }

    [Fact]  
    public async Task Test()
    {
        var service = Services.GetRequiredService<IService>();
        
        service.DoSomething();
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