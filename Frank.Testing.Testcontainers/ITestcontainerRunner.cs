using DotNet.Testcontainers.Containers;

namespace Frank.Testing.Testcontainers;

public interface ITestcontainerRunner : IAsyncDisposable
{
    Task StartAsync();
    
    Task StopAsync();
    
    TestcontainersStates GetState();
    
    Task ExecuteCommandAsync(string command, CancellationToken cancellationToken = default);
    
    Task ExecuteAsync(Func<Task> actionAsync);
}