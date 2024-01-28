using DotNet.Testcontainers.Containers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Frank.Testing.Testcontainers;

public class ContainerRunner<T> : ITestcontainerRunner where T : class, IContainer
{
    private readonly ILogger<T> _logger;
    private readonly T _container;
    private readonly CancellationToken _cancellationToken;
    private readonly TimeSpan _timeout;
    
    private CancellationTokenSource? _cancellationTokenSource;
    
    internal ContainerRunner(ILogger<T>? logger, T container, TimeSpan timeout, CancellationToken cancellationToken)
    {
        _logger = logger ??= new NullLogger<T>();
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _cancellationToken = cancellationToken;
        _timeout = timeout;
    }

    public async Task StartAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource(_timeout);
        _cancellationToken.Register(() => _cancellationTokenSource.Cancel());
        await _container.StartAsync(_cancellationTokenSource.Token);
    }
    
    public async Task StopAsync()
    {
        await _container.StopAsync(_cancellationToken);
        await DisposeAsync();
    }

    public TestcontainersStates GetState() => _container.State;

    /// <inheritdoc />
    public async Task ExecuteCommandAsync(string command, CancellationToken cancellationToken = default)
    {
        await _container.ExecAsync(new[] { command }, _cancellationToken);
    }

    public async Task ExecuteAsync(Func<Task> actionAsync)
    {
        try
        {
            await actionAsync();
        }
        catch (Exception? exception)
        {
            _logger.LogError(exception, "Failed to execute action in container {ContainerName}", _container.Name);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
        _cancellationTokenSource?.Dispose();
    }
}