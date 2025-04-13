using Frank.Testing.TestBases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.Testing.Tests.TestBases;

[HostApplicationDependencyInjectionDataProvider]
public class HostApplicationDependencyInjectionDataProviderAttributeBaseTests(IMyService myService)
{
    [Test]
    public async Task Test()
    {
        var cancellationToken = CancellationToken.None;
        await myService.RunAsync(cancellationToken);
    }
}

public class HostApplicationDependencyInjectionDataProviderAttribute : HostApplicationDependencyInjectionDataProviderAttributeBase
{
    /// <inheritdoc />
    public override HostApplicationBuilder Setup(HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMyService, MyService>();
        
        return builder;
    }
}

public class MyService : IMyService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running MyService");
        return Task.CompletedTask;
    }
}

public interface IMyService
{
    Task RunAsync(CancellationToken cancellationToken);
}