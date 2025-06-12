using _305.Application.Base.Response;
using _305.Application.IUOW;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;

namespace _305.Application.Base.Handler;

/// <summary>
/// هندلر عمومی برای عملیات حذف موجودیت.
/// از UnitOfWork برای مدیریت تراکنش استفاده می‌کند و قابل استفاده در سناریوهای مختلف حذف است.
/// </summary>
public class DeleteHandler
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger _logger;

	/// <summary>
	/// سازنده هندلر حذف با تزریق UnitOfWork و Logger.
	/// </summary>
	/// <param name="unitOfWork">واحد کاری برای مدیریت تراکنش‌ها</param>
	public DeleteHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_logger = Log.ForContext<DeleteHandler>();
	}

	/// <summary>
	/// اجرای عملیات حذف موجودیت با واکشی، بررسی وجود، حذف و Commit.
	/// </summary>
	/// <typeparam name="TEntity">نوع موجودیت مورد نظر برای حذف</typeparam>
	/// <typeparam name="TResult">نوع خروجی پاسخ</typeparam>
	/// <param name="findEntityAsync">تابعی برای واکشی موجودیت</param>
	/// <param name="onBeforeDeleteAsync">تابع دلخواه برای اعمال عملیات پیش از حذف</param>
	/// <param name="onDeleteAsync">تابع حذف (در صورت نیاز به تغییر رفتار پیش‌فرض)</param>
	/// <param name="entityName">نام موجودیت برای پیام‌ها</param>
	/// <param name="notFoundMessage">پیام دلخواه هنگام عدم یافتن</param>
	/// <param name="successMessage">پیام دلخواه هنگام موفقیت</param>
	/// <param name="successStatusCode">کد وضعیت موفقیت HTTP (پیش‌فرض: 204)</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات به صورت <see cref="ResponseDto{TResult}"/></returns>
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
			// یافتن موجودیت
			var entity = await findEntityAsync();
			if (entity == null)
			{
				var message = notFoundMessage ?? $"{entityName} یافت نشد";
				return Responses.NotFound<TResult>(default, entityName, message);
			}

			// عملیات اختیاری قبل از حذف
			onBeforeDeleteAsync?.Invoke(entity);

			// عملیات حذف (در صورت نیاز)
			onDeleteAsync?.Invoke(entity);

			var committed = await _unitOfWork.CommitAsync(cancellationToken);
			return !committed ? Responses.ExceptionFail<TResult>(default, $"{entityName} ویرایش نشد", 500) : Responses.ChangeOrDelete<TResult>(default, $"{entityName} با موفقیت ویرایش شد");
		}
		catch (OperationCanceledException)
		{
			_logger.Warning("عملیات حذف لغو شد توسط CancellationToken");
			return Responses.Fail<TResult>(default, "عملیات لغو شد", 499);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "خطا در حذف {EntityName}: {Message}", entityName, ex.Message);
			return Responses.ExceptionFail<TResult>(default, "خطا در حذف " + entityName);
		}
	}
}
