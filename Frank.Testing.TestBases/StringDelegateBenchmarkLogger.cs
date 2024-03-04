using System.Text;

using BenchmarkDotNet.Loggers;

namespace Frank.Testing.TestBases;

public class StringDelegateBenchmarkLogger(Action<string> output) : ILogger
{
    private readonly StringBuilder _stringBuilder = new();

    /// <inheritdoc />
    public void Write(LogKind logKind, string text) => _stringBuilder.Append(text);

    /// <inheritdoc />
    public void WriteLine()
    {
        output(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }

    /// <inheritdoc />
    public void WriteLine(LogKind logKind, string text) => _stringBuilder.AppendLine(text);

    /// <inheritdoc />
    public void Flush()
    {
        output(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }

    /// <inheritdoc />
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Priority { get; set; } = 0;
}