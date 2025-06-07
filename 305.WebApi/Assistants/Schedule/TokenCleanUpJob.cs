using _305.Application.IUOW;
using _305.WebApi.Assistants.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace _305.WebApi.Assistants.Schedule;

/// <summary>
/// Job زمان‌بندی‌شده برای پاک‌سازی توکن‌های منقضی‌شده.
/// </summary>
public class TokenCleanUpJob(
	IRecurringJobManager recurringJobManager,
	IServiceScopeFactory scopeFactory)
{
	public const string JobId = "token-cleanup";

	/// <summary>
	/// تنظیم زمان‌بندی اجرای متناوب Job با Hangfire (به‌صورت ساعتی).
	/// </summary>
	public void ConfigureJobs()
	{
		recurringJobManager.AddOrUpdate(
			JobId,
			() => ExecuteScopedTask(),
			Cron.Hourly
		);
	}

	/// <summary>
	/// اجرای کار در یک Scope ایزوله‌شده برای دریافت سرویس‌ها به‌صورت Scoped.
	/// </summary>
	public async Task ExecuteScopedTask()
	{
		using var scope = scopeFactory.CreateScope();

		var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		var tokenCleanupTask = scope.ServiceProvider.GetRequiredService<TokenCleanupTask>();

		await tokenCleanupTask.ExecuteAsync(unitOfWork);
	}
}