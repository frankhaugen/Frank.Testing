using System.Diagnostics;
using System.Text;

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

using BenchmarkDotNetVisualizer;

using Frank.Testing.Logging;
using Frank.Testing.TestBases;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.TestBases;

public class HostApplicationBenchmarkBaseTests : HostApplicationBenchmarkBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    /// <inheritdoc />
    public HostApplicationBenchmarkBaseTests(ITestOutputHelper testOutputHelper) : base(new SimpleTestLoggerProvider(testOutputHelper))
    {
        _testOutputHelper = testOutputHelper;
    } 
    
    
    [Fact]
    public async Task Test1()
    {
        // Arrange
        
        // Act
        var summary = RunBenchmarks<TestBenchmark>(new DebugInProcessConfig()
            .AddDiagnoser(MemoryDiagnoser.Default, new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig()), ThreadingDiagnoser.Default)
            .AddExporter(HtmlExporter.Default, CsvExporter.Default, MarkdownExporter.GitHub)
            .AddAnalyser(OutliersAnalyser.Default, EnvironmentAnalyser.Default, MultimodalDistributionAnalyzer.Default, ZeroMeasurementAnalyser.Default)
            .AddHardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions, HardwareCounter.CacheMisses, HardwareCounter.TotalCycles, HardwareCounter.Timer)
            .AddLogger(new StringDelegateBenchmarkLogger(_testOutputHelper.WriteLine))
        );
        
        // Assert
        // var html = summary.GetMarkdown(new ReportMarkdownOptions()
        // {
        //     Title = "TestBenchmark"
        // });
        //
        // _testOutputHelper.WriteLine(html);
    }
    
    [HardwareCounters(
        HardwareCounter.BranchMispredictions,
        HardwareCounter.BranchInstructions)]
    public class TestBenchmark
    {
        [Benchmark]
        public void Run()
        {
            var result = 1f + 1f + 1f;
        }
    }
}

public class XUnitBenchmarkConfiguration : ManualConfig
{
    public XUnitBenchmarkConfiguration(ILogger? benchmarkLogger = null)
    {
        if (benchmarkLogger != null) 
            AddLogger(benchmarkLogger);
        
        AddJob(Job.ShortRun);
        AddExporter(HtmlExporter.Default, new RichMarkdownExporter(new ReportMarkdownOptions()
        {
            Title = "Benchmark"
        }));
        AddDiagnoser(MemoryDiagnoser.Default, new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig()), ThreadingDiagnoser.Default);
        WithOptions(ConfigOptions.DisableOptimizationsValidator);
    }
    

}
