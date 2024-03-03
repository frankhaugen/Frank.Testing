namespace Frank.Testing.Logging;

public class InMemoryLoggerScope<T> : IDisposable
{
    public T? State { get; private set; }
    
    public InMemoryLoggerScope(object state) => State = state is T t ? t : throw new ArgumentException($"The state must be of type {typeof(T).Name}");

    /// <inheritdoc />
    public void Dispose() => State = default;
}