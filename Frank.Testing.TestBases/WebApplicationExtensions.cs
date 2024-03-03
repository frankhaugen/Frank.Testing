using Microsoft.AspNetCore.Builder;

namespace Frank.Testing.TestBases;

internal static class WebApplicationExtensions
{
    public static HttpClient CreateTestClient(this WebApplication application) =>
        new()
        {
            BaseAddress = new Uri(application.Urls.FirstOrDefault() ?? throw new InvalidOperationException("Base address for TestClient has not been initialized yet."), UriKind.Absolute)
        };
}