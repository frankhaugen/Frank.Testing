# Frank.Testing

A set of nugets to allow for easier testing of dotnet code using xUnit

___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.Testing)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.Testing.svg)](https://www.nuget.org/packages/Frank.Testing)
[![NuGet](https://img.shields.io/nuget/dt/Frank.Testing.svg)](https://www.nuget.org/packages/Frank.Testing)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.Testing)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.Testing)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.Testing)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.Testing)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.Testing)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.Testing)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.Testing)
___

## Installation

### NuGet

```powershell
Install-Package Frank.Testing
```

### .NET CLI

```bash
dotnet add package Frank.Testing
```

## Usage

### TestOutputHelperExtensions

```csharp
using Xunit;
using Xunit.Abstractions;

public class MyTestClass
{
    private readonly ITestOutputHelper _outputHelper;

    public MyTestClass(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void MyTestMethod()
    {
        _outputHelper.WriteLine(new { MyProperty = "MyValue" });
    }
}
```

