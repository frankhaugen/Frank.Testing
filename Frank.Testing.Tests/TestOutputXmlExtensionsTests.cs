using JetBrains.Annotations;

using Xunit.Abstractions;

namespace Frank.Testing.Tests;

[TestSubject(typeof(TestOutputXmlExtensions))]
public class TestOutputXmlExtensionsTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void WriteXml_ShouldWriteXmlOutput_WhenSourceIsSimpleObject()
    {
        var source = new TestPerson { Name = "John Doe", Age = 30 };

        outputHelper.WriteXml(source);
    }

    [Fact]
    public void WriteXml_ShouldWriteXmlOutput_WhenSourceIsNestedObject()
    {
        var source = new TestPerson() { Name = "John Doe", Age = 30, Address = new TestAddress() { City = "Main City", ZipCode = 18846 } };
        outputHelper.WriteXml(source);
    }

    [Fact]
    public void WriteXml_ShouldHandleEmptyObjects()
    {
        var source = new TestPerson();
        
        outputHelper.WriteXml(source);
    }

    [Fact]
    public void WriteXml_ShouldHandleNullObjects()
    {
        object? source = null;
        
        outputHelper.WriteXml(source);
    }
}