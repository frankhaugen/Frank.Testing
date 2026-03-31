using Frank.Testing.TestServer;

using Microsoft.AspNetCore.Http;

namespace Frank.Testing.Tests.TestServer;

public class TestApiHostTests
{
    [Test]
    public async Task TestApiHost_CanStartAndStop()
    {
        // Arrange
        var host = TestApiHost.Create()
            .WithMiddleware(request =>
            {
                // Middleware logs to test output writer
                TestContext.Current!.OutputWriter.WriteLine($"Request: {request.Method}");
                return request;
            })
            .With(HttpMethod.Get, "/test", async context =>
            {
                // Simulate some processing
                await Task.Delay(100);
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Hello, World!");
            })
            .Build(new Uri("http://localhost:5511"));

        // Act
        await host.App.StartAsync();
        var response = await host.Client.GetAsync("/test");
        await host.App.StopAsync();

        // Assert
        await Assert.That(response.IsSuccessStatusCode).IsTrue();
    }
}