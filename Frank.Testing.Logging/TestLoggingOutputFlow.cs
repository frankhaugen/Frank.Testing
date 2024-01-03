using Frank.PulseFlow;
using Frank.PulseFlow.Logging;

using Xunit.Abstractions;

namespace Frank.Testing.Logging;

/// <summary>
/// Represents a class that handles logging output flow.
/// </summary>
public class TestLoggingOutputFlow(ITestOutputHelper outputHelper) : IFlow
{
    public Task HandleAsync(IPulse pulse, CancellationToken cancellationToken)
    {
        outputHelper.WriteLine(pulse.ToString());
        return Task.CompletedTask;
    }

    public bool CanHandle(Type pulseType) => pulseType == typeof(LogPulse<>);
}