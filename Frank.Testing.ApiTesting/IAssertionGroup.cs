namespace Frank.Testing.ApiTesting;

public interface IAssertionGroup
{
    string GroupName { get; set; }
    
    SortedList<uint, IAssertion> Assertions { get; set; }
}