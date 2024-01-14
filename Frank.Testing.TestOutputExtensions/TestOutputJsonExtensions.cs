using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xunit.Abstractions;

public static class TestOutputJsonExtensions
{
    /// <summary>
    /// Writes a JSON representation of the specified object to the output helper.
    /// </summary>
    /// <typeparam name="T">The type of object to be serialized.</typeparam>
    /// <param name="outputHelper">The output helper to write the JSON to.</param>
    /// <param name="source">The object to be serialized.</param>
    /// <param name="options">The options to be used for the serialization (optional).</param>
    /// <remarks>
    /// If <paramref name="options"/> is not provided, the default <see cref="JsonSerializerOptions"/> will be used.
    /// </remarks>
    public static void WriteJson<T>(this ITestOutputHelper outputHelper, T? source, JsonSerializerOptions? options = null) => outputHelper.WriteLine(options == null ? JsonSerializer.Serialize(source, JsonSerializerOptions) : JsonSerializer.Serialize(source, options));

    /// <summary>
    /// Gets the default JsonSerializerOptions.
    /// </summary>
    /// <returns>The default JsonSerializerOptions.</returns>
    public static JsonSerializerOptions GetDefaultJsonSerializerOptions(this ITestOutputHelper _) => JsonSerializerOptions;
    
    private static JsonSerializerOptions JsonSerializerOptions => new() { Converters = { new JsonStringEnumConverter() }, WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}