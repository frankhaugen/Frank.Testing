using ConsoleTableExt;

namespace Xunit.Abstractions;

public static class TestOutputTableExtensions
{
    public static void WriteTable<T>(this TestContext? outputHelper, IEnumerable<T> source, ConsoleTableBuilderFormat format = ConsoleTableBuilderFormat.Minimal) where T : class =>
        outputHelper?.OutputWriter.WriteLine(ConsoleTableBuilder
            .From(source.ToList())
            .WithFormat(format)
            .Export()
            .ToString());
}
