using DotNet.Testcontainers.Containers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Frank.Testing.Testcontainers;

public class TestContainerRunnerBuilder<T> where T : class, IContainer
{
    private ILogger<T>? _logger;
    private TimeSpan _maxLifetime;
    private CancellationToken _cancellationToken;
    private Func<IContainer>? _containerFactory;

    public TestContainerRunnerBuilder<T> WithLogger(ILogger<T>? logger)
    {
        _logger = logger;
        return this;
    }

    public TestContainerRunnerBuilder<T> WithMaxLifetime(TimeSpan maxLifetime)
    {
        _maxLifetime = maxLifetime;
        return this;
    }

    public TestContainerRunnerBuilder<T> WithCancellationToken(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return this;
    }

    public TestContainerRunnerBuilder<T> WithContainerFactory(Func<IContainer>? containerFactory)
    {
        _containerFactory = containerFactory;
        return this;
    }

    public ITestcontainerRunner Build()
    {
        _logger ??= new NullLogger<T>();
        if (_containerFactory == null)
            throw new ArgumentNullException(nameof(_containerFactory));
        if (_maxLifetime == default)
            _maxLifetime = TimeSpan.FromMinutes(1);
        if (_cancellationToken == default)
            _cancellationToken = CancellationToken.None;
        
        return new ContainerRunner<IContainer>(_logger, _containerFactory(), _maxLifetime, _cancellationToken);
    }
}