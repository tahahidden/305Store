using _305.Application.Base.Response;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class DeleteHandlerTestHelper
{
	/// <summary>
	/// این متد به صورت عمومی یک تست حذف (Delete) را برای هندلرهای دستور (Command Handler) انجام می‌دهد.
	/// با استفاده از موک UnitOfWork و Repository، عملکرد حذف موجودیت را شبیه‌سازی و بررسی می‌کند.
	/// </summary>
	/// <typeparam name="TCommand">نوع دستور حذف</typeparam>
	/// <typeparam name="TEntity">نوع موجودیت که حذف می‌شود (باید IBaseEntity را پیاده‌سازی کند)</typeparam>
	/// <typeparam name="TRepository">نوع ریپازیتوری که مدیریت TEntity را برعهده دارد</typeparam>
	/// <typeparam name="THandler">نوع هندلر دستور حذف</typeparam>
	/// <param name="handlerFactory">تابعی که هندلر را با دریافت UnitOfWork می‌سازد</param>
	/// <param name="execute">تابعی که هندلر و دستور را گرفته و نتیجه اجرای حذف را بازمی‌گرداند</param>
	/// <param name="command">دستور حذف که به هندلر ارسال می‌شود</param>
	/// <param name="repoSelector">عبارتی که مشخص می‌کند کدام ریپازیتوری از UnitOfWork باید استفاده شود</param>
	/// <param name="setupRepoMock">عملیاتی اختیاری برای تنظیمات اضافی روی موک ریپازیتوری</param>
	public static async Task TestDelete<TCommand, TEntity, TRepository, THandler>(
		Func<IUnitOfWork, THandler> handlerFactory,
		Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
		TCommand command,
		Expression<Func<IUnitOfWork, TRepository>> repoSelector,
		Action<Mock<TRepository>>? setupRepoMock = null
	)
		where TEntity : class, IBaseEntity
		where TRepository : class, IRepository<TEntity>
		where THandler : class
	{
		// ساخت موک از UnitOfWork و Repository مربوطه
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		// تنظیم UnitOfWork برای برگشت ریپازیتوری موک شده هنگام فراخوانی repoSelector
		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		// ایجاد یک نمونه موک از موجودیت
		var entity = Mock.Of<TEntity>();

		// تنظیم موک ریپازیتوری برای متد FindSingle که موجودیت را برمی‌گرداند (شبیه‌سازی یافتن موجودیت برای حذف)
		repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync(entity);

		// تنظیم متد Remove که حذف موجودیت را شبیه‌سازی می‌کند
		repoMock.Setup(r => r.Remove(It.IsAny<TEntity>()));

		// تنظیم متد CommitAsync از UnitOfWork برای تأیید تراکنش (بازگشت true به معنی موفقیت‌آمیز بودن)
		unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

		// اگر تنظیمات اضافی برای موک ریپازیتوری نیاز باشد، اجرا می‌شود
		setupRepoMock?.Invoke(repoMock);

		// ساخت هندلر با UnitOfWork موک شده
		var handler = handlerFactory(unitOfWorkMock.Object);

		// اجرای متد حذف و دریافت نتیجه
		var result = await execute(handler, command, CancellationToken.None);

		// بررسی اینکه عملیات موفق بوده است
		Assert.True(result.is_success);

		// بررسی اینکه کد پاسخ HTTP برابر 204 (No Content) است که معمولاً برای حذف موفق استفاده می‌شود
		Assert.Equal(204, result.response_code);

		// بررسی اینکه متد Remove روی ریپازیتوری حداقل یکبار صدا زده شده است
		repoMock.Verify(r => r.Remove(It.IsAny<TEntity>()), Times.Once);

		// بررسی اینکه متد CommitAsync از UnitOfWork حداقل یکبار فراخوانی شده است
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	/// <summary>
	/// این متد تست حذف زمانی را انجام می‌دهد که موجودیت مورد نظر برای حذف در دیتابیس پیدا نشود.
	/// در این حالت انتظار می‌رود که حذف موفق نباشد و کد پاسخ 404 برگردد.
	/// </summary>
	/// <typeparam name="TCommand">نوع دستور حذف</typeparam>
	/// <typeparam name="TEntity">نوع موجودیت که حذف می‌شود</typeparam>
	/// <typeparam name="TRepository">نوع ریپازیتوری مدیریت کننده موجودیت</typeparam>
	/// <typeparam name="THandler">نوع هندلر دستور حذف</typeparam>
	/// <param name="handlerFactory">تابعی برای ساخت هندلر با UnitOfWork</param>
	/// <param name="execute">تابعی برای اجرای متد حذف روی هندلر</param>
	/// <param name="command">دستور حذف</param>
	/// <param name="repoSelector">عبارت انتخاب ریپازیتوری از UnitOfWork</param>
	public static async Task TestDeleteNotFound<TCommand, TEntity, TRepository, THandler>(
		Func<IUnitOfWork, THandler> handlerFactory,
		Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
		TCommand command,
		Expression<Func<IUnitOfWork, TRepository>> repoSelector
	)
		where TEntity : class, IBaseEntity
		where TRepository : class, IRepository<TEntity>
		where THandler : class
	{
		// ساخت موک از UnitOfWork و Repository
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var repoMock = new Mock<TRepository>();

		// تنظیم UnitOfWork برای بازگرداندن ریپازیتوری موک شده
		unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

		// تنظیم FindSingle به گونه‌ای که موجودیتی پیدا نشود (برگرداندن null)
		repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync((TEntity?)null);

		// ساخت هندلر
		var handler = handlerFactory(unitOfWorkMock.Object);

		// اجرای متد حذف
		var result = await execute(handler, command, CancellationToken.None);

		// بررسی اینکه عملیات حذف موفق نبوده است
		Assert.False(result.is_success);

		// بررسی اینکه کد پاسخ HTTP برابر 404 (Not Found) باشد
		Assert.Equal(404, result.response_code);
	}
}

