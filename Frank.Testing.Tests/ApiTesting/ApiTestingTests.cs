using System.Net;

using Frank.Testing.ApiTesting;

using NSubstitute;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.ApiTesting;

public class ApiTestingTests
{
    private readonly ITestOutputHelper _outputHelper;

    public ApiTestingTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public async Task Test1()
    {
        var harness = new ApiTestingHarness(new MockHttpClientFactory());

        var assertionGroup1 = new AssertionGroup
        {
            GroupName = "Group 1",
            Assertions = new List<IAssertion>
                {
                    new Assertion
                    {
                        Name = "Is BRREG API alive?",
                        Endpoint = new Uri("https://data.brreg.no"),
                        Method = HttpMethod.Get,
                        Timeout = TimeSpan.FromSeconds(10),
                        ExpectedResponseCode = HttpStatusCode.OK
                    },
                    new Assertion
                    {
                        Name = "Can I download something from the BRREG API?",
                        Endpoint = new Uri("https://data.brreg.no/enhetsregisteret/api/enheter"),
                        Method = HttpMethod.Get,
                        Timeout = TimeSpan.FromSeconds(10),
                        ExpectedResponseCode = HttpStatusCode.OK
                    }
                }
            };

            var assertionGroup2 = new AssertionGroup
            {
            GroupName = "Group 2",
            Assertions = new List<IAssertion>
            {
            // Add more assertions...
            }
        };

        harness.AssertionGroups = new List<AssertionGroup> { assertionGroup1, assertionGroup2 };

        var results = await harness.RunAssertionsAsync();

        foreach (var result in results)
        {
            _outputHelper.WriteLine($"Assertion: {result.AssertionName}");
            _outputHelper.WriteLine($"Success: {result.IsSuccess}");
            _outputHelper.WriteLine($"Elapsed time: {result.ElapsedTime}");
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                _outputHelper.WriteLine($"Error Message: {result.ErrorMessage}");
            }
            _outputHelper.WriteLine("");
        }

        _outputHelper.WriteLine("This is a test");
        
        Assert.True(results.All(arg => arg.IsSuccess));
    }

    private class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new();
    }
}