namespace Frank.Testing.ApiTesting;

public class AssertionGroup
{
    public string GroupName { get; set; }
    public List<IAssertion> Assertions { get; set; }
}