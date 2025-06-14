using _305.WebApi.Assistants.Permission;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// Hosted service برای همگام‌سازی مجوزها هنگام راه‌اندازی برنامه.
/// </summary>
public class PermissionSeedHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
            await seeder.SyncPermissionsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "error during seeding permissions");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

