using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

[TestSubject(typeof(TestOutputJsonExtensions))]
public class TestOutputExtensionsTests
{
    private readonly ITestOutputHelper _outputHelper;

    public TestOutputExtensionsTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void Test1()
    {
        var model = new TestModel { Name = "Frank" };
        _outputHelper.WriteLine(model);
    }
    
    [Fact]
    public void Test2()
    {
        var model = new TestModel { Name = "Frank" };
        _outputHelper.WriteJson(model);
    }
    
    private class TestModel
    {
        public string? Name { get; set; }
    }
}