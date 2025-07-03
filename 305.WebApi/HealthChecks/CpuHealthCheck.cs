using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace _305.WebApi.HealthChecks;

public class CpuHealthCheck : IHealthCheck
{
    private readonly double _maxUsagePercentage;

    public CpuHealthCheck(double maxUsagePercentage = 85)
    {
        _maxUsagePercentage = maxUsagePercentage;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        using var proc = System.Diagnostics.Process.GetCurrentProcess();
        var totalCpuTime = proc.TotalProcessorTime.TotalSeconds;
        var uptime = (DateTime.UtcNow - proc.StartTime.ToUniversalTime()).TotalSeconds;
        var cpuUsage = uptime > 0 ? (totalCpuTime / (uptime * Environment.ProcessorCount)) * 100 : 0;

        var status = cpuUsage < _maxUsagePercentage ? HealthStatus.Healthy : HealthStatus.Degraded;
        var description = $"CPU usage: {cpuUsage:F2}%";
        return Task.FromResult(new HealthCheckResult(status, description));
    }
}
