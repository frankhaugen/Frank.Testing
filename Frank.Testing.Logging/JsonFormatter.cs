using System.Text.Json;

namespace Frank.Testing.Logging;

public class JsonFormatter
{
    public static string Format<TState>(TState state, Exception? exception)
    {
        return JsonSerializer.Serialize(state);
    }
}