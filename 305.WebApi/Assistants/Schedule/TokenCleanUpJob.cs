using _305.Application.IUOW;
using _305.WebApi.Assistants.Tasks;
using Hangfire;

namespace _305.WebApi.Assistants.Schedule;

public class TokenCleanUpJob
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IServiceScopeFactory _scopeFactory;

    public const string JobId = "token-cleanup";

    public TokenCleanUpJob(IRecurringJobManager recurringJobManager, IServiceScopeFactory scopeFactory)
    {
        _recurringJobManager = recurringJobManager;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// زمان‌بندی اجرای خودکار Job به صورت متناوب.
    /// </summary>
    public void ConfigureJobs()
    {
        _recurringJobManager.AddOrUpdate(
            JobId,
            () => ExecuteScopedTask(),
            cronExpression: Cron.Hourly
        );
    }

    /// <summary>
    /// اجرای Job در محدوده Scope ایزوله‌شده.
    /// </summary>
    /// <returns></returns>
    public async Task ExecuteScopedTask()
    {
        using var scope = _scopeFactory.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var tokenCleanupTask = scope.ServiceProvider.GetRequiredService<TokenCleanupTask>();

        await tokenCleanupTask.ExecuteAsync(unitOfWork);
    }
}