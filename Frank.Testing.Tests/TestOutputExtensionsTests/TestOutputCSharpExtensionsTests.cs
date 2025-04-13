using Frank.Testing.Tests.TestingInfrastructure;

using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

public class TestOutputCSharpExtensionsTests()
{
    [Test]
    public void WriteCSharp_ShouldWriteCSharpOutput_WhenSourceIsSimpleObject()
    {
        var source = new TestPerson { Name = "John Doe", Age = 30 };

        TestContext.Current.WriteCSharp(source);
    }
}