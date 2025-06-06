using _305.Application.IRepository;
using _305.Application.IUOW;
using Moq;

namespace _305.Tests.Unit.Assistant;

// todo
public static class MockUnitOfWorkFactory
{
    /// <summary>
    /// این متد یک نمونه موک‌شده از IUnitOfWork می‌سازد که در تست‌های واحد کاربرد دارد.
    /// می‌توانید موک‌های سفارشی از ریپازیتوری‌ها را به عنوان پارامتر به این متد ارسال کنید،
    /// یا اگر ارسال نکنید، خودش به صورت پیش‌فرض موک جدید می‌سازد.
    /// </summary>
    /// <param name="blogRepositoryMock">موک دلخواه از IBlogRepository (اختیاری)</param>
    /// <param name="blogCategoryRepositoryMock">موک دلخواه از IBlogCategoryRepository (اختیاری)</param>
    /// <returns>نمونه موک شده IUnitOfWork با ریپازیتوری‌های تنظیم شده</returns>
    /// TODO : USE THIS MAYBE
    public static Mock<IUnitOfWork> Create(
        Mock<IBlogRepository>? blogRepositoryMock = null,
        Mock<IBlogCategoryRepository>? blogCategoryRepositoryMock = null)
    {
        // ایجاد یک موک جدید از IUnitOfWork
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // اگر موک ریپازیتوری برای بلاگ ارسال نشده بود، یک موک جدید بساز
        blogRepositoryMock ??= new Mock<IBlogRepository>();

        // اگر موک ریپازیتوری برای دسته‌بندی بلاگ ارسال نشده بود، یک موک جدید بساز
        blogCategoryRepositoryMock ??= new Mock<IBlogCategoryRepository>();

        // تنظیم مقدار برگشتی پراپرتی BlogRepository روی موک ارسال شده یا ساخته شده
        unitOfWorkMock.Setup(u => u.BlogRepository).Returns(blogRepositoryMock.Object);

        // تنظیم مقدار برگشتی پراپرتی BlogCategoryRepository روی موک ارسال شده یا ساخته شده
        unitOfWorkMock.Setup(u => u.BlogCategoryRepository).Returns(blogCategoryRepositoryMock.Object);

        // تنظیم متد CommitAsync که در زمان صدا زدن با هر CancellationToken، مقدار true برمی‌گرداند
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        // بازگرداندن نمونه موک‌شده IUnitOfWork
        return unitOfWorkMock;
    }
}
