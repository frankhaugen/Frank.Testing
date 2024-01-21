using System.ComponentModel;

using VarDump;
using VarDump.Visitor;

namespace Xunit.Abstractions;

public static class TestOutputCSharpExtensions
{
    /// <summary>
    /// Writes the provided source object as C# code to the test output.
    /// </summary>
    /// <remarks>
    /// The C# code is written using the <see cref="CSharpDumper"/>
    /// </remarks>
    /// <example>
    /// <code>
    /// var testPerson = new Frank.Testing.Tests.TestPerson
    /// {
    ///     Name = "John Doe",
    ///     Age = 30,
    ///     Address = new Frank.Testing.Tests.TestAddress
    ///     {
    ///         City = "Main City",
    ///         ZipCode = 18846
    ///     }
    /// };
    /// </code>
    /// </example>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="outputHelper">The test output helper.</param>
    /// <param name="source">The source object to be written as C# code.</param>
    /// <param name="dumpOptions">The optional dump options to control the formatting of the C# code.</param>
    public static void WriteCSharp<T>(this ITestOutputHelper outputHelper, T source, DumpOptions? dumpOptions = null)
    {
        var options = dumpOptions ?? DumpOptions;
        var dumper = new CSharpDumper(options);
        outputHelper.WriteLine(dumper.Dump(source));
    }

    private static DumpOptions DumpOptions => new DumpOptions()
    {
        IgnoreNullValues = true,
        DateKind = DateKind.ConvertToUtc,
        DateTimeInstantiation = DateTimeInstantiation.Parse,
        UseTypeFullName = true,
        GenerateVariableInitializer = true,
        SortDirection = ListSortDirection.Ascending,
        MaxDepth = 64,
    };
}