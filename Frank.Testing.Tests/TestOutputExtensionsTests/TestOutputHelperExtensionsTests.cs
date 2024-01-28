using Frank.Testing.Logging;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

[TestSubject(typeof(TestOutputHelperExtensions))]
public class TestOutputHelperExtensionsTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void CreateTestLoggerAndLog()
    {
        //Arrange
        var loggerString = testOutputHelper.CreateTestLogger<string>();

        //Act
        loggerString.LogInformation("Hello World");

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerDefaultLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>();

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerTraceLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Trace);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerDebugLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Debug);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerInformationLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Information);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerWarningLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Warning);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerErrorLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Error);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerCriticalLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.Critical);

        //Assert
        Assert.NotNull(loggerString);
    }

    [Fact]
    public void CreateTestLoggerNoneLogLevel()
    {
        //Arrange
        //Act
        var loggerString = testOutputHelper.CreateTestLogger<string>(LogLevel.None);

        //Assert
        Assert.NotNull(loggerString);
    }
}