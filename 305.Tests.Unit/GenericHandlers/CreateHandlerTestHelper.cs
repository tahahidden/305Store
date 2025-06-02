using _305.Application.Base.Response;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using _305.Tests.Unit.Assistant;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class CreateHandlerTestHelper
{
	public static async Task TestCreateSuccess<TCommand, TEntity, TRepository, THandler>(
	Func<IUnitOfWork, THandler> handlerFactory,
	Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
	TCommand command,
	Expression<Func<IUnitOfWork, TRepository>> repoSelector,
	Action<Mock<TRepository>>? setupRepoMock = null,
	string? expectedNameForExistsCheck = null // مقدار مورد انتظار برای چک کردن شرط ExistsAsync
	)
	where TEntity : class, IBaseEntity
	where TRepository : class, IRepository<TEntity>
	where THandler : class
	{
		// ایجاد mock از UnitOfWork و Repository
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		// وصل کردن Repository mock به UnitOfWork mock
		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		// اگر مقدار مورد انتظار برای شرط چک موجودیت داده شده، 
		// شرط ExistsAsync را با بررسی ExpressionHelper تنظیم کن
		if (!string.IsNullOrEmpty(expectedNameForExistsCheck))
		{
			repoMock.Setup(r => r.ExistsAsync(It.Is<Expression<Func<TEntity, bool>>>(expr =>
				TestExpressionEvaluator.MatchSlugExpression<TEntity>(expr, expectedNameForExistsCheck)
			))).ReturnsAsync(false);
		}
		else
		{
			// پیش‌فرض: موجودیت مشابه وجود ندارد
			repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(false);
		}

		// شبیه‌سازی افزودن موجودیت موفق
		repoMock.Setup(r => r.AddAsync(It.IsAny<TEntity>())).Returns(Task.CompletedTask);

		// شبیه‌سازی Commit موفق
		unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

		// اعمال تنظیمات اضافی روی repoMock (در صورت ارسال)
		setupRepoMock?.Invoke(repoMock);

		// ساخت Handler با UnitOfWork mock
		var handler = handlerFactory(unitOfWorkMock.Object);

		// اجرای Command روی Handler و گرفتن نتیجه
		var result = await execute(handler, command, CancellationToken.None);

		// بررسی نتیجه موفقیت‌آمیز بودن عملیات
		Assert.True(result.is_success);
		Assert.Equal(201, result.response_code);
		Assert.NotNull(result.data);

		// بررسی اینکه AddAsync دقیقاً یک بار فراخوانی شده
		repoMock.Verify(r => r.AddAsync(It.IsAny<TEntity>()), Times.Once);

		// بررسی اینکه CommitAsync دقیقاً یک بار فراخوانی شده
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}


	public static async Task TestCreateFailure<TCommand, TEntity, TRepository, THandler>(
	   Func<IUnitOfWork, THandler> handlerFactory, // تابعی برای ساخت Handler با UnitOfWork
	   Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute, // تابعی برای اجرای Command روی Handler
	   TCommand command, // کامندی که باید تست شود
	   Expression<Func<IUnitOfWork, TRepository>> repoSelector, // مشخص‌کننده‌ی اینکه از UnitOfWork کدام Repository گرفته شود
	   Action<Mock<TRepository>>? setupRepoMock = null, // تنظیمات سفارشی روی Mock ریپازیتوری
	   string? expectedMessage = null // پیام خطایی که انتظار داریم در خروجی باشد
   )
	   where TEntity : class, IBaseEntity
	   where TRepository : class, IRepository<TEntity>
	   where THandler : class
	{
		// ساخت mock از UnitOfWork و Repository
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		// وصل کردن Repository mock به UnitOfWork mock
		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		// پیش‌فرض: موجودیت مشابه در دیتابیس وجود ندارد (برای کنترل بهتر با setupRepoMock قابل تغییر است)
		repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(false);

		// شبیه‌سازی commit موفق (اگر به هر دلیلی اجرا شد)
		unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

		// اجرای تنظیمات سفارشی اگر وجود داشته باشد
		setupRepoMock?.Invoke(repoMock);

		// ساخت Handler با UnitOfWork mock
		var handler = handlerFactory(unitOfWorkMock.Object);

		// اجرای command روی handler
		var result = await execute(handler, command, CancellationToken.None);

		// بررسی اینکه عملیات موفق نبوده
		Assert.False(result.is_success);

		// بررسی اینکه کد بازگشتی 201 (Created) نیست
		Assert.NotEqual(201, result.response_code);

		// بررسی اینکه پیام خطا شامل پیام مورد انتظار باشد (اگر تعیین شده)
		if (!string.IsNullOrEmpty(expectedMessage))
		{
			Assert.Contains(expectedMessage, result.message ?? "", StringComparison.OrdinalIgnoreCase);
		}

		// بررسی اینکه متد AddAsync اصلاً صدا زده نشده (در حالت خطا انتظار نداریم چیزی ذخیره شود)
		repoMock.Verify(r => r.AddAsync(It.IsAny<TEntity>()), Times.Never);

		// بررسی اینکه Commit اصلاً صدا زده نشده (چون چیزی تغییر نکرده)
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
	}


	public static async Task TestCreateException<TCommand, TEntity, TRepository, THandler>(
	 Func<IUnitOfWork, THandler> handlerFactory, // تابعی برای ساخت Handler
	 Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute, // تابع اجرای handler
	 TCommand command, // کامند مورد تست
	 Expression<Func<IUnitOfWork, TRepository>> repoSelector, // مشخص‌کننده‌ی اینکه کدام Repository استفاده شود
	 Action<Mock<TRepository>>? setupRepoMock = null // تنظیمات سفارشی برای mock
 )
	 where TEntity : class, IBaseEntity
	 where TRepository : class, IRepository<TEntity>
	 where THandler : class
	{
		// ایجاد mock برای UnitOfWork و Repository
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		// اتصال Repository mock به UnitOfWork mock
		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		// اعمال تنظیمات سفارشی در صورت وجود
		setupRepoMock?.Invoke(repoMock);

		// ساخت Handler با UnitOfWork mock
		var handler = handlerFactory(unitOfWorkMock.Object);

		// اجرای command و گرفتن نتیجه (احتمالاً شامل Exception مدیریت‌شده)
		var result = await execute(handler, command, CancellationToken.None);

		// بررسی اینکه نتیجه موفقیت‌آمیز نیست
		Assert.False(result.is_success);

		// بررسی اینکه کد خطای 500 برگشته (فرض: Exception باعث Internal Server Error شده)
		Assert.Equal(500, result.response_code);

		// بررسی اینکه پیام خطا وجود دارد
		Assert.NotNull(result.message);

		// در این حالت بررسی اینکه Add یا Commit انجام شده یا نه بستگی به محل رخ دادن خطا دارد،
		// پس اینجا بررسی نمی‌کنیم و دست توسعه‌دهنده باز است.
	}


}
