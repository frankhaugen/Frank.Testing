using System.Text.Json;

namespace Xunit.Abstractions;

public static class TestOutputExtensions
{
    /// <summary>
    /// Writes the specified object's string representation followed by the current line terminator to the output.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="outputHelper">The ITestOutputHelper instance.</param>
    /// <param name="source">The object to write.</param>
    /// <exception cref="System.Text.Json.JsonException">Thrown when unable to serialize the object.</exception>
    public static void WriteLine<T>(this ITestOutputHelper outputHelper, T? source) => outputHelper.WriteLine(JsonSerializer.Serialize(source, outputHelper.GetDefaultJsonSerializerOptions()));
}