using Frank.Testing.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.Testing.TestBases;

public abstract class HostApplicationDependencyInjectionDataProviderAttributeBase : DependencyInjectionDataSourceAttribute<IServiceScope>
{
    private IHost? Host;

    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata)
    {
        Host ??= CreateHost();
        return Host.Services.CreateAsyncScope();
    }

    public override object? Create(IServiceScope scope, Type type)
    {
        Host ??= CreateHost();
        return scope.ServiceProvider.GetService(type);
    }
    
    private IHost CreateHost()
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
        builder = Setup(builder);
        builder.Logging.AddDebug().AddSimpleTestLogger();
        return builder.Build();
    }
    
    public virtual HostApplicationBuilder Setup(HostApplicationBuilder builder)
    {
        // This method can be overridden to add services to the HostApplicationBuilder
        // and configure the service provider as needed.
        return builder;
    }
}