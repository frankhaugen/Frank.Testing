using Frank.Testing.Logging;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

namespace Frank.Testing.Tests.TestOutputExtensionsTests;

[TestSubject(typeof(TestOutputHelperExtensions))]
public class TestOutputHelperExtensionsTests()
{
    [Test]
    public async Task CreateTestLoggerAndLog()
    {
        //Arrange
        var loggerString = TestContext.Current.CreateTestLogger<string>();

        //Act
        loggerString.LogInformation("Hello World");

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerDefaultLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>();

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerTraceLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Trace);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerDebugLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Debug);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerInformationLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Information);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerWarningLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Warning);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerErrorLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Error);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerCriticalLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.Critical);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }

    [Test]
    public async Task CreateTestLoggerNoneLogLevel()
    {
        //Arrange
        //Act
        var loggerString = TestContext.Current.CreateTestLogger<string>(LogLevel.None);

        //Assert
        await Assert.That(loggerString).IsNotNull();
    }
}