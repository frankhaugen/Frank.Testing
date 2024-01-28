using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

public class TestOutputTableExtensionsTests
{
    private readonly ITestOutputHelper _outputHelper;

    public TestOutputTableExtensionsTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void ToTable_WithEnumerable_ReturnsTable()
    {
        // Arrange
        var passwords = new[]
        {
            new { Sha1Hash = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", Sha1Prefix = "5BAA6", Sha2Suffix = "1E4C9B93F3F0682250B6CF8331B7EE68FD8", TimesPwned = 3645844 }, new { Sha1Hash = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", Sha1Prefix = "5BAA6", Sha2Suffix = "1E4C9B93F3F0682250B6CF8331B7EE68FD8", TimesPwned = 3645844 }, new { Sha1Hash = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", Sha1Prefix = "5BAA6", Sha2Suffix = "1E4C9B93F3F0682250B6CF8331B7EE68FD8", TimesPwned = 3645844 },
            new { Sha1Hash = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", Sha1Prefix = "5BAA6", Sha2Suffix = "1E4C9B93F3F0682250B6CF8331B7EE68FD8", TimesPwned = 364584 },
        };

        // Act
        _outputHelper.WriteTable(passwords);
    }
}