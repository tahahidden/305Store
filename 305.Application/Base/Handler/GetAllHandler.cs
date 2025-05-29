using _305.Application.Base.Response;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای دریافت لیست تمامی موجودیت‌ها و تبدیل آن‌ها به DTO.
/// این کلاس به صورت Generic پیاده‌سازی شده و از الگوی Unit of Work و AutoMapper استفاده می‌کند.
/// </summary>
public class GetAllHandler
{
	private readonly IUnitOfWork _unitOfWork;

	/// <summary>
	/// سازنده کلاس که وابستگی به UnitOfWork را تزریق می‌کند.
	/// </summary>
	/// <param name="unitOfWork">واحد کاری برای دسترسی به مخازن داده</param>
	public GetAllHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	/// <summary>
	/// اجرای عملیات دریافت لیست موجودیت‌ها و تبدیل آن‌ها به DTO
	/// </summary>
	/// <typeparam name="TEntity">نوع موجودیت دیتابیس (Entity)</typeparam>
	/// <typeparam name="TDto">نوع شیء انتقال داده (Data Transfer Object)</typeparam>
	/// <param name="entities">لیستی از موجودیت‌ها که قبلاً دریافت شده‌اند</param>
	/// <returns>
	/// شیء <see cref="ResponseDto{List{TDto}}"/> شامل لیست DTOها یا خطای احتمالی
	/// </returns>
	public ResponseDto<List<TDto>> Handle<TEntity, TDto>(
		List<TEntity> entities)
		where TEntity : class
		where TDto : class, new()
	{
		try
		{
			// تبدیل هر آیتم در لیست به صورت مجزا
			var dtoList = entities.Select(e => Mapper.Mapper.Map<TEntity, TDto>(e)).ToList();

			// بازگرداندن لیست به صورت پاسخ موفق
			return Responses.Data(dtoList);
		}
		catch (Exception ex)
		{
			// لاگ‌گیری مستقیم با Serilog در صورت بروز خطا
			Log.Error(ex, "خطا در زمان تبدیل لیست موجودیت‌ها به DTO: {Message}", ex.Message);
			return Responses.ExceptionFail<List<TDto>>(default, null);
		}
	}
}
