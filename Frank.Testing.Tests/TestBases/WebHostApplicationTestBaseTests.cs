using FluentAssertions;

using Frank.Testing.Logging;
using Frank.Testing.TestBases;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class WebHostApplicationTestBaseTests(ITestOutputHelper outputHelper) : WebApplicationTestBase(outputHelper, loggerProvider: new InMemoryLoggerProvider(Options.Create(new LoggerFilterOptions() {MinLevel = LogLevel.Debug})))
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    /// <inheritdoc />
    protected override Task SetupAsync(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddApplicationPart(typeof(MyController).Assembly); // Add controllers from the assembly where MyController is defined
        builder.Services.AddHealthChecks();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task SetupApplicationAsync(WebApplication application)
    {
        application.MapControllers();
        application.MapHealthChecks("/health");
        application.MapGet("/test1", async httpContext =>
            {
                await httpContext.Response.WriteAsync("Test1 endpoint");
            });
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test()
    {
        var response = await GetTestClient.GetAsync("/test1");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Test1 endpoint");
    }
    
    [Fact]
    public async Task Test2()
    {
        _outputHelper.WriteLine("Endpoints:");
        foreach (var endpointRoute in GetEndpointRoutes) _outputHelper.WriteLine(endpointRoute);
        
        var response = await GetTestClient.GetAsync("/test2");
        var content = await response.Content.ReadAsStringAsync();
        // outputHelper.WriteLine(response);
        content.Should().Be("Test2 endpoint");
    }
}

[ApiController]
public class MyController : ControllerBase
{
    [HttpGet("test2")]
    public IActionResult Test2()
    {
        return Ok("Test2 endpoint");
    }
}