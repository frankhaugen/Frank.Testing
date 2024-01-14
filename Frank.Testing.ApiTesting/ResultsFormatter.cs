namespace Frank.Testing.ApiTesting;

using System.Collections.Generic;
using System.Xml.Linq;

public class ResultsFormatter
{
    public string FormatResults(List<ResultGroup> groups)
    {
        var xDocument = new XDocument();
        var root = new XElement("TestGroups");

        foreach (var group in groups)
        {
            var groupElement = new XElement("Group",
                new XAttribute("Name", group.GroupName));

            foreach (var assertion in group.AssertionResults)
            {
                var testElement = new XElement("Test",
                    new XElement("Name", assertion.AssertionName),
                    new XElement("Success", assertion.IsSuccess),
                    new XElement("ErrorMessage", assertion.ErrorMessage ?? string.Empty),
                    new XElement("ElapsedTime", assertion.ElapsedTime.ToString("g"))
                );

                groupElement.Add(testElement);
            }

            root.Add(groupElement);
        }

        xDocument.Add(root);
        return xDocument.ToString();
    }
}
