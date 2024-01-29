using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.Testing.TestBases;

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