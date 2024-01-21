namespace Frank.Testing.Tests.TestingInfrastructure;

public class TestPerson
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    
    public TestAddress? Address { get; set; }
}