using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace _305.WebApi.HealthChecks;

public class MemoryHealthCheck : IHealthCheck
{
    private readonly long _maxBytes;

    public MemoryHealthCheck(long maxBytes = 1024 * 1024 * 1024) // 1 GB default
    {
        _maxBytes = maxBytes;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var allocated = GC.GetTotalMemory(false);
        var status = allocated < _maxBytes ? HealthStatus.Healthy : HealthStatus.Degraded;
        var description = $"Allocated memory: {allocated / (1024 * 1024)} MB";
        return Task.FromResult(new HealthCheckResult(status, description));
    }
}
