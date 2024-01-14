using Frank.Testing.Tests.TestingInfrastructure;

using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

[TestSubject(typeof(TestOutputCSharpExtensions))]
public class TestOutputCSharpExtensionsTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void WriteCSharp_ShouldWriteCSharpOutput_WhenSourceIsSimpleObject()
    {
        var source = new TestPerson { Name = "John Doe", Age = 30 };

        outputHelper.WriteCSharp(source);
    }
}