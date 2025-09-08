using _305.WebApi.Assistants.Permission;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// Hosted service برای همگام‌سازی مجوزها هنگام راه‌اندازی برنامه.
/// </summary>
public class PermissionSeedHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionSeedHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
        await seeder.SyncPermissionsAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

