using Frank.PulseFlow;
using Frank.PulseFlow.Logging;
using Frank.Testing.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _outputHelper;

    public UnitTest1(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void Test1()
    {
        var host = CreateHostBuilder().Build();
        
        host.Start();
    }
    
    
    
    private IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddPulseFlow();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITestOutputHelper>(_outputHelper);
                services.AddPulseFlow(builder =>
                {
                    builder.AddFlow<TestLoggingOutputFlow>();
                });
                
                services.AddHostedService<MyService>();
            });
    }

    private class MyService : BackgroundService
    {
        private readonly ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger) => _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hello from MyService");
            await Task.Delay(1000, stoppingToken);
        }
    }
}