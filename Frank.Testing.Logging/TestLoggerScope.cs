namespace Frank.Testing.Logging;

public class TestLoggerScope<TState>(TState state) : IDisposable
{
    public TState? State { get; private set; } = state;

    public void Dispose() => State = default;
}