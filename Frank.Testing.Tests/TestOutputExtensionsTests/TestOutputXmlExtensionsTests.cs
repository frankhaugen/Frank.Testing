using Frank.Testing.Tests.TestingInfrastructure;

using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

public class TestOutputXmlExtensionsTests()
{
    [Test]
    public void WriteXml_ShouldWriteXmlOutput_WhenSourceIsSimpleObject()
    {
        var source = new TestPerson { Name = "John Doe", Age = 30 };

        TestContext.Current.WriteXml(source);
    }

    [Test]
    public void WriteXml_ShouldWriteXmlOutput_WhenSourceIsNestedObject()
    {
        var source = new TestPerson() { Name = "John Doe", Age = 30, Address = new TestAddress() { City = "Main City", ZipCode = 18846 } };
        TestContext.Current.WriteXml(source);
    }

    [Test]
    public void WriteXml_ShouldHandleEmptyObjects()
    {
        var source = new TestPerson();
        
        TestContext.Current.WriteXml(source);
    }

    [Test]
    public void WriteXml_ShouldHandleNullObjects()
    {
        object? source = null;
        
        TestContext.Current.WriteXml(source);
    }
}