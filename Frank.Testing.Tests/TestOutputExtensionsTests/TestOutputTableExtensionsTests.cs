
using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

public class TestOutputTableExtensionsTests
{
    public class PwnageRRecorrd
    {
        public string Sha1Hash { get; }
        public string Sha1Prefix { get; }
        public string Sha2Suffix { get; }
        public int TimesPwned { get; }

        public PwnageRRecorrd(string sha1Hash, string sha1Prefix, string sha2Suffix, int timesPwned)
        {
            Sha1Hash = sha1Hash;
            Sha1Prefix = sha1Prefix;
            Sha2Suffix = sha2Suffix;
            TimesPwned = timesPwned;
        }
    }

    [Test]
    public void ToTable_WithEnumerable_ReturnsTable()
    {
        // Arrange
        var passwords = new[]
        {
            new PwnageRRecorrd("5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", "5BAA6", "1E4C9B93F3F0682250B6CF8331B7EE68FD8", 3645844),
            new PwnageRRecorrd("5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", "5BAA6", "1E4C9B93F3F0682250B6CF8331B7EE68FD8", 3645844),
            new PwnageRRecorrd("5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", "5BAA6", "1E4C9B93F3F0682250B6CF8331B7EE68FD8", 3645844),
            new PwnageRRecorrd("5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8", "5BAA6", "1E4C9B93F3F0682250B6CF8331B7EE68FD8", 364584),
        };

        // Act
        TestContext.Current?.WriteTable(passwords);
    }
}