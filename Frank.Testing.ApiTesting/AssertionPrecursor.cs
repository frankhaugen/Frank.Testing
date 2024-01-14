using System.Net;

namespace Frank.Testing.ApiTesting;

public class AssertionPrecursor<TResponse>
{
    public string? Name { get; set; }
    
    public Uri Endpoint { get; set; }

    public HttpMethod Method { get; set; }

    public TimeSpan Timeout { get; set; }
    
    public TResponse? ExpectedResponse { get; set; }

    public HttpStatusCode ExpectedResponseCode { get; set; }
}