using _305.Application.Base.Response;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای عملیات حذف (Delete) موجودیت‌ها در لایه داده.
/// از الگوی Unit of Work پشتیبانی می‌کند و قابلیت استفاده مجدد در سناریوهای مختلف را دارد.
/// </summary>
/// <remarks>
/// این کلاس حذف موجودیت را با اعمال فانکشن دلخواه برای پیدا کردن و حذف موجودیت انجام می‌دهد.
/// در صورت عدم یافتن موجودیت، پاسخ NotFound باز می‌گردد.
/// در صورت موفقیت، عملیات Commit انجام می‌شود و پیام موفقیت بازمی‌گردد.
/// </remarks>
public class DeleteHandler
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger _logger;
	/// <summary>
	/// سازنده کلاس با تزریق UnitOfWork.
	/// </summary>
	/// <param name="unitOfWork">واحد کاری برای مدیریت تراکنش‌ها</param>
	public DeleteHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_logger = Log.ForContext<DeleteHandler>();
	}

	/// <summary>
	/// اجرای عملیات حذف موجودیت با مدیریت تراکنش و بررسی وجود موجودیت.
	/// </summary>
	/// <typeparam name="TEntity">نوع موجودیت مورد نظر برای حذف</typeparam>
	/// <typeparam name="TResult">نوع خروجی دلخواه</typeparam>
	/// <param name="findEntityAsync">تابعی برای واکشی موجودیت مورد نظر</param>
	/// <param name="onBeforeDeleteAsync">عملیات حذف که روی موجودیت اعمال می‌شود</param>
	/// <param name="onDeleteAsync">فانکشن حذف</param>
	/// <param name="entityName">نام موجودیت برای پیام‌های خطا یا موفقیت</param>
	/// <param name="notFoundMessage">پیام دلخواه در صورت عدم یافتن موجودیت</param>
	/// <param name="successMessage">پیام دلخواه در صورت حذف موفق</param>
	/// <param name="successStatusCode">کد وضعیت HTTP برای موفقیت، به‌صورت پیش‌فرض 204 (NoContent)</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>شیء <see cref="ResponseDto{TResult}"/> شامل نتیجه عملیات</returns>
	public async Task<ResponseDto<TResult>> HandleAsync<TEntity, TResult>(
		Func<Task<TEntity?>> findEntityAsync,
		Action<TEntity>? onBeforeDeleteAsync = null,
		Action<TEntity>? onDeleteAsync = null,
		string? entityName = "آیتم",
		string? notFoundMessage = null,
		string? successMessage = null,
		int successStatusCode = 204,
		CancellationToken cancellationToken = default)
		where TEntity : class
	{
		try
		{
			var entity = await findEntityAsync();
			if (entity == null)
				return Responses.NotFound<TResult>(default, entityName, notFoundMessage ?? $"{entityName} یافت نشد");

			onBeforeDeleteAsync?.Invoke(entity);

			onDeleteAsync?.Invoke(entity);

			await _unitOfWork.CommitAsync(cancellationToken);

			return Responses.ChangeOrDelete<TResult>(default, successMessage ?? $"{entityName} با موفقیت حذف شد", successStatusCode);
		}
		catch (OperationCanceledException)
		{
			_logger.Warning("عملیات ایجاد لغو شد توسط CancellationToken");
			return Responses.Fail<TResult>(default, "عملیات لغو شد", 499);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "خطا در حذف {EntityName}: {Message}", entityName, ex.Message);
			return Responses.ExceptionFail<TResult>(default, null);
		}
	}
}
