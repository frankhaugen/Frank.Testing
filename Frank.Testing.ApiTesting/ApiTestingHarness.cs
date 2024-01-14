using System.Diagnostics;

namespace Frank.Testing.ApiTesting;

public class ApiTestingHarness
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiTestingHarness(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<AssertionGroup> AssertionGroups { get; set; }

    public async Task<List<Result>> RunAssertionsAsync()
    {
        var assertionResults = new List<Result>();

        foreach (var group in AssertionGroups)
        {
            var tasks = new List<Task<Result>>();

            foreach (var assertion in group.Assertions)
            {
                tasks.Add(RunAssertionAsync(assertion));
            }

            var results = await Task.WhenAll(tasks);

            assertionResults.AddRange(results);
        }

        return assertionResults;
    }

    private async Task<Result> RunAssertionAsync(IAssertion assertion)
    {
        var result = new Result
        {
            AssertionName = assertion.Name,
            IsSuccess = false,
            ErrorMessage = string.Empty
        };

        var stopwatch = Stopwatch.StartNew();
        try
        {
            using var client = _httpClientFactory.CreateClient();
            client.Timeout = assertion.Timeout;

            var request = new HttpRequestMessage(assertion.Method, assertion.Endpoint);
            request.Content = assertion.RequestContent;

            var response = await client.SendAsync(request);

            if (response.StatusCode == assertion.ExpectedResponseCode)
            {
                if (assertion.ExpectedResponseContent != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent == await assertion.ExpectedResponseContent.ReadAsStringAsync())
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.ErrorMessage = "Response content does not match expected content.";
                    }
                }
                else
                {
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.ErrorMessage = $"Expected response code {assertion.ExpectedResponseCode}, but received {response.StatusCode}.";
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.Message;
        }
        stopwatch.Stop();
        
        result.ElapsedTime = stopwatch.Elapsed;

        return result;
    }
}