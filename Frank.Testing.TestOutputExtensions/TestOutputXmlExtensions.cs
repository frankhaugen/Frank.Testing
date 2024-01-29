using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Xunit.Abstractions;

public static class TestOutputXmlExtensions
{
    public static void WriteXml<T>(this ITestOutputHelper outputHelper, T source, XmlWriterSettings? xmlWriterSettings = null)
    {
        var settings = xmlWriterSettings ?? XmlWriterSettings;
        
        using var textWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(textWriter, settings);
        var xmlSerializer = new XmlSerializerFactory().CreateSerializer(typeof(T));
        xmlSerializer.Serialize(xmlWriter, source, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
        outputHelper.WriteLine(textWriter.ToString());
    }

    private static XmlWriterSettings XmlWriterSettings => new()
    {
        Indent = true,
        IndentChars = new string(' ', 4),
        NewLineChars = "\n",
        NewLineHandling = NewLineHandling.Replace,
        OmitXmlDeclaration = false,
        Encoding = new UTF8Encoding(false)
    };
}