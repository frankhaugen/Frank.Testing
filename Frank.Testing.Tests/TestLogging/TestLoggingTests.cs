using Frank.Testing.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestLogging;

public class TestLoggingTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task Test1()
    {
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var host = CreateHostBuilder().Build();
        
        await host.RunAsync(cancellationTokenSource.Token);
    }
    
    private IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging
                    .AddPulseFlowTestLoggingProvider(outputHelper, LogLevel.Information)
                    // .AddSimpleTestLoggingProvider(outputHelper, LogLevel.Information)
                    ;
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<MyService>();
            });
    }

    private class MyService(ILogger<MyService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = logger.BeginScope("Titans are {Description}", "awesome");
            var counter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Hello from MyService {Counter}", counter++);
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}