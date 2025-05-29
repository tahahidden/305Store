using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای عملیات ایجاد (Create) موجودیت‌ها در لایه داده.
/// از الگوی Unit of Work و اعتبارسنجی سفارشی پشتیبانی می‌کند.
/// </summary>
/// <remarks>
/// این هندلر قابلیت استفاده مجدد در سناریوهای مختلف ایجاد (Create) را فراهم می‌کند.
/// با استفاده از لیستی از <see cref="ValidationItem"/> ابتدا بررسی می‌کند که آیا شرایطی مانع ایجاد موجودیت هست یا خیر.
/// در صورت اعتبارسنجی موفق، عملیات ایجاد از طریق delegate <paramref name="onCreate"/> انجام شده
/// و سپس Commit به پایگاه داده صورت می‌گیرد.
/// </remarks>
public class CreateHandler
{
	private readonly IUnitOfWork _unitOfWork;

	/// <summary>
	/// سازنده کلاس با تزریق UnitOfWork.
	/// </summary>
	public CreateHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	/// <summary>
	/// اجرای عملیات ایجاد موجودیت با اعتبارسنجی و مدیریت تراکنش.
	/// </summary>
	/// <typeparam name="TResult">نوع نتیجه خروجی</typeparam>
	/// <param name="validations">لیست قوانین اعتبارسنجی برای بررسی تکراری بودن یا شرایط خاص</param>
	/// <param name="onCreate">تابع حاوی منطق ایجاد موجودیت</param>
	/// <param name="createMessage">پیام موفقیت در صورت موفق بودن عملیات</param>
	/// <param name="cancellationToken">توکن کنسل کردن عملیات</param>
	/// <returns>شیء <see cref="ResponseDto{TResult}"/> شامل نتیجه عملیات</returns>
	public async Task<ResponseDto<TResult>> HandleAsync<TResult>(
		List<ValidationItem>? validations,
		Func<Task<TResult>> onCreate,
		string? createMessage = "عملیات موفق بود",
		CancellationToken cancellationToken = default)
	{
		if (validations != null)
		{
			foreach (var validation in validations)
			{
				var isValid = await validation.Rule();
				if (isValid)
				{
					return Responses.Exist<TResult>(default, null, validation.Value);
				}
			}
		}

		try
		{
			var result = await onCreate();
			await _unitOfWork.CommitAsync(cancellationToken);
			return Responses.Success(result, createMessage, 201);
		}
		catch (Exception ex)
		{
			// لاگ‌گیری با Serilog برای ثبت خطاهای غیرمنتظره
			Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<TResult>(default, null);
		}
	}
}
