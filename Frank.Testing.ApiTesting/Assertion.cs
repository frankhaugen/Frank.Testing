using System.Net;

namespace Frank.Testing.ApiTesting;

public class Assertion<TRequest, TResponse>
{
    public string? Name { get; set; }
    
    public Uri Endpoint { get; set; }

    public HttpMethod Method { get; set; }

    public TimeSpan Timeout { get; set; }

    public TRequest? Request { get; set; }
    
    public TResponse? ExpectedResponse { get; set; }

    public HttpStatusCode ExpectedResponseCode { get; set; }
}

public class Assertion : IAssertion
{
    public string? Name { get; set; }

    public Uri Endpoint { get; set; }

    public HttpMethod Method { get; set; }

    public TimeSpan Timeout { get; set; }

    public HttpContent? RequestContent { get; set; }

    public HttpContent? ExpectedResponseContent { get; set; }

    public HttpStatusCode ExpectedResponseCode { get; set; }
}