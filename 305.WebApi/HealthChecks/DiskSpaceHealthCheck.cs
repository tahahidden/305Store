using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IO;
using System.Linq;

namespace _305.WebApi.HealthChecks;

public class DiskSpaceHealthCheck : IHealthCheck
{
    private readonly long _minFreeBytes;
    private readonly DriveInfo _drive;

    public DiskSpaceHealthCheck(string driveName, long minFreeBytes = 1024 * 1024 * 1024)
    {
        _drive = DriveInfo.GetDrives().First(d => d.Name.StartsWith(driveName, StringComparison.OrdinalIgnoreCase));
        _minFreeBytes = minFreeBytes;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var free = _drive.AvailableFreeSpace;
        var status = free > _minFreeBytes ? HealthStatus.Healthy : HealthStatus.Degraded;
        var description = $"Free space on {_drive.Name}: {free / (1024 * 1024)} MB";
        return Task.FromResult(new HealthCheckResult(status, description));
    }
}
