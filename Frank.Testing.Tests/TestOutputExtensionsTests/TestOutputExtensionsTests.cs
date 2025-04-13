using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

public class TestOutputExtensionsTests
{
    [Test]
    public void Test1()
    {
        var model = new TestModel { Name = "Frank" };
        TestContext.Current.WriteLine(model);
    }
    
    [Test]
    public void Test2()
    {
        var model = new TestModel { Name = "Frank" };
        TestContext.Current.WriteJson(model);
    }
    
    private class TestModel
    {
        public string? Name { get; set; }
    }
}