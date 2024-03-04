using System.Globalization;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Extensions;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Perfolizer.Horology;

namespace Frank.Testing.TestBases;

public abstract class HostApplicationBenchmarkBase : HostApplicationTestBase
{
    /// <inheritdoc />
    protected HostApplicationBenchmarkBase(ILoggerProvider loggerProvider) : base(loggerProvider, LogLevel.Information)
    {
    }
    
    /// <summary>
    /// Run a benchmark of the specified type and return the summary.
    /// </summary>
    /// <returns></returns>
    protected Summary RunBenchmarks<T>(IConfig config) where T : class
    {
        return BenchmarkRunner.Run<T>(config);
    }
}