using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.Testing.TestBases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(TService)).ToList();
        if (!descriptors.Any())
            return services;// throw new InvalidOperationException($"No service of type {typeof(TService)} has been registered.");

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
            var clone = new ServiceDescriptor(typeof(TService), typeof(TImplementation), descriptor.Lifetime);
            services.Add(clone);
        }

        return services;
    }
}
    
public static class WebHostExtensions
{
    public static HttpClient CreateTestClient(this IWebHost host)
    {
        var baseAddress = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.First();
        
        return new HttpClient
        {
            BaseAddress = new Uri(baseAddress ?? throw new InvalidOperationException("The host has not been initialized yet."), UriKind.Absolute)
        };
    }
    
    public static IEnumerable<string?> GetServerEndpoints(this IWebHost host)
    {
        return host.Services.GetServices<EndpointDataSource>().SelectMany(x => x.Endpoints).Select(x => x.DisplayName);
    }
}