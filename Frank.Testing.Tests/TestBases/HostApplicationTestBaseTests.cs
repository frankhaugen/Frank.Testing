using FluentAssertions;
using FluentAssertions.Common;

using Frank.Testing.Logging;
using Frank.Testing.TestBases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Frank.Testing.Tests.TestBases;

public class HostApplicationTestBaseTests : HostApplicationTestBase
{
    [Before(HookType.Test)]
    public virtual void BeforeEveryTest()
    {
        InitializeAsync().GetAwaiter().GetResult();
        StartAsync().GetAwaiter().GetResult();
    }
    
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<TestService>();
        builder.Services.AddSingleton<TestService2>();
        return Task.CompletedTask;
    }

    [Test]
    public async Task Test()
    {
        await Task.Delay(1500);
    }
    
    [Test]
    public void Test2()
    {
        var service = GetServices.GetService<TestService2>();
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