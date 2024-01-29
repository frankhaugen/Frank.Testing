using FluentAssertions;

using Frank.Testing.TestBases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class HostApplicationTestBaseTests(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<TestService>();
        builder.Services.AddSingleton<TestService2>();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test()
    {
        await Task.Delay(2500);
    }
    
    [Fact]
    public void Test2()
    {
        var service = Services.GetService<TestService2>();
        service.Should().NotBeNull();
    }
    
    public class TestService2
    {
        public TestService2(ILogger<TestService2> logger)
        {
            logger.LogInformation("Foo");
        }
    }
    
    public class TestService(ILogger<TestService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                logger.LogInformation("Hello, world!");
            }
        }
    }
}