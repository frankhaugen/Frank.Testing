using System.Net;

using FluentAssertions;

using Frank.Testing.TestBases;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class WebHostApplicationTestBaseTests(ITestOutputHelper outputHelper) : WebHostApplicationTestBase(outputHelper)
{
    protected override Task SetupAsync(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddControllersWithViews();
        });
        builder.Configure((context, app) =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
                endpoints.MapGet("/test1", async httpContext =>
                {
                    await httpContext.Response.WriteAsync("Test1 endpoint");
                });
                
            });
        });
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test()
    {
        var response = await TestClient.GetAsync("/test1");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Test1 endpoint");
    }
    
    [Fact]
    public async Task Test2()
    {
        var response = await TestClient.GetAsync("/test2");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Test2 endpoint");
    }
    
    [Fact]
    public void Test3()
    {
        var endpoints = GetServerEndpoints();
        // endpoints = Enumerable.Empty<string>();
        outputHelper.WriteLine(string.Join(Environment.NewLine, endpoints));
    }
}

public class MyController : ControllerBase
{
    [HttpGet("/test2")]
    public IActionResult Test2()
    {
        return Ok("Test2 endpoint");
    }
}