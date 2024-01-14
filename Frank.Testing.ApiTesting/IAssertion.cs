using System.Net;

namespace Frank.Testing.ApiTesting;

public interface IAssertion
{
    string? Name { get; set; }
    Uri Endpoint { get; set; }
    HttpMethod Method { get; set; }
    TimeSpan Timeout { get; set; }
    HttpContent? RequestContent { get; set; }
    HttpContent? ExpectedResponseContent { get; set; }
    HttpStatusCode ExpectedResponseCode { get; set; }
}