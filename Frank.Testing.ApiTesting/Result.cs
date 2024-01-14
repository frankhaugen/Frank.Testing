namespace Frank.Testing.ApiTesting;

public class Result
{
    public string AssertionName { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan ElapsedTime { get; set; }
    
    public HttpContent? ResponseContent { get; set; }
}