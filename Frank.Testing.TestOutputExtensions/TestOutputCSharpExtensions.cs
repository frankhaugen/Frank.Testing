using System.ComponentModel;

using Frank.Reflection.Dump;

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
        outputHelper.WriteLine(source.DumpClass(options));
    }

    /// <summary>
    /// Writes the C# representation of the elements in the specified source collection to the test output.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <param name="outputHelper">The test output helper.</param>
    /// <param name="source">The source collection.</param>
    /// <param name="idSelector">The function to extract an identifier from each element.</param>
    /// <param name="dumpOptions">The options for dumping the elements. Null to use the default options.</param>
    public static void WriteCSharp<T>(this ITestOutputHelper outputHelper, IEnumerable<T> source, Func<T, string> idSelector, DumpOptions? dumpOptions = null)
    {
        var options = dumpOptions ?? DumpOptions;
        outputHelper.WriteLine(source.DumpEnumerable(idSelector, options));
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