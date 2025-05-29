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

	/// <summary>
	/// سازنده کلاس با تزریق UnitOfWork.
	/// </summary>
	/// <param name="unitOfWork">واحد کاری برای مدیریت تراکنش‌ها</param>
	public DeleteHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	/// <summary>
	/// اجرای عملیات حذف موجودیت با مدیریت تراکنش و بررسی وجود موجودیت.
	/// </summary>
	/// <typeparam name="TEntity">نوع موجودیت مورد نظر برای حذف</typeparam>
	/// <typeparam name="TResult">نوع خروجی دلخواه</typeparam>
	/// <param name="findEntity">تابعی برای واکشی موجودیت مورد نظر</param>
	/// <param name="onDelete">عملیات حذف که روی موجودیت اعمال می‌شود</param>
	/// <param name="name">نام موجودیت برای پیام‌های خطا یا موفقیت</param>
	/// <param name="notFoundMessage">پیام دلخواه در صورت عدم یافتن موجودیت</param>
	/// <param name="successMessage">پیام دلخواه در صورت حذف موفق</param>
	/// <param name="successCode">کد وضعیت HTTP برای موفقیت، به‌صورت پیش‌فرض 204 (NoContent)</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>شیء <see cref="ResponseDto{TResult}"/> شامل نتیجه عملیات</returns>
	public async Task<ResponseDto<TResult>> HandleAsync<TEntity, TResult>(
		Func<Task<TEntity?>> findEntity,
		Action<TEntity> onDelete,
		string? name = "آیتم",
		string? notFoundMessage = "آیتم مورد نظر پیدا نشد",
		string? successMessage = "آیتم با موفقیت حذف شد",
		int successCode = 204,
		CancellationToken cancellationToken = default)
		where TEntity : class
	{
		try
		{
			var entity = await findEntity();
			if (entity == null)
				return Responses.NotFound<TResult>(default, name, notFoundMessage);

			onDelete(entity);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Responses.ChangeOrDelete<TResult>(default, successMessage, successCode);
		}
		catch (Exception ex)
		{
			// لاگ‌گیری با Serilog برای ثبت خطای زمان حذف
			Log.Error(ex, "خطا در زمان حذف موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<TResult>(default, null);
		}
	}
}
