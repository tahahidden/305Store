using _305.Application.IRepository;
using _305.Application.IUOW;
using Moq;

namespace _305.Tests.Unit.Assistant;

/// <summary>
/// کلاس کمکی برای ساخت نمونه‌های موک‌شده از IUnitOfWork و ریپازیتوری‌های مرتبط.
/// این متد به راحتی امکان جایگزینی موک‌های دلخواه ریپازیتوری‌ها را دارد.
/// </summary>
public static class MockUnitOfWorkFactory
{
	/// <summary>
	/// ایجاد نمونه موک‌شده از IUnitOfWork با امکان ارسال موک‌های سفارشی برای ریپازیتوری‌ها.
	/// اگر موک خاصی ارسال نشود، خودش موک جدید می‌سازد.
	/// </summary>
	/// <param name="blogRepositoryMock">موک سفارشی IBlogRepository (اختیاری)</param>
	/// <param name="blogCategoryRepositoryMock">موک سفارشی IBlogCategoryRepository (اختیاری)</param>
	/// <returns>نمونه موک‌شده IUnitOfWork با ریپازیتوری‌های تنظیم شده</returns>
	public static Mock<IUnitOfWork> Create(
		Mock<IBlogRepository>? blogRepositoryMock = null,
		Mock<IBlogCategoryRepository>? blogCategoryRepositoryMock = null)
	{
		// ساخت موک از IUnitOfWork
		var unitOfWorkMock = new Mock<IUnitOfWork>();

		// اگر موک ریپازیتوری بلاگ ارسال نشده بود، یک موک جدید بساز
		blogRepositoryMock ??= new Mock<IBlogRepository>();

		// اگر موک ریپازیتوری دسته‌بندی بلاگ ارسال نشده بود، یک موک جدید بساز
		blogCategoryRepositoryMock ??= new Mock<IBlogCategoryRepository>();

		// تنظیم ریپازیتوری‌ها در موک IUnitOfWork
		unitOfWorkMock.Setup(u => u.BlogRepository).Returns(blogRepositoryMock.Object);
		unitOfWorkMock.Setup(u => u.BlogCategoryRepository).Returns(blogCategoryRepositoryMock.Object);

		// تنظیم متد CommitAsync برای بازگرداندن مقدار true برای هر CancellationToken
		unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

		return unitOfWorkMock;
	}
}