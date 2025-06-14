using _305.Application.Base.Response;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class GetAllHandlerTestHelper
{
    /// <summary>
    /// تست موفقیت‌آمیز هندلری که لیستی از داده‌ها را برمی‌گرداند
    /// با استفاده از موک UnitOfWork و Repository، داده‌های نمونه را شبیه‌سازی و اجرای هندلر را بررسی می‌کند.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت که IBaseEntity را پیاده‌سازی کرده</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری که IRepository<TEntity> است</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابعی برای ساخت هندلر با ورودی UnitOfWork</param>
    /// <param name="execute">تابعی که هندلر و توکن لغو می‌گیرد و نتیجه اجرای هندلر را برمی‌گرداند</param>
    /// <param name="repoSelector">اکسپریشن برای انتخاب ریپازیتوری از UnitOfWork</param>
    /// <param name="entities">لیست نمونه موجودیت‌ها برای بازگشت توسط ریپازیتوری</param>
    /// <param name="setupRepoMock">اکشن اختیاری برای تنظیمات اضافی روی موک ریپازیتوری</param>
    public static async Task TestHandle_Success<TEntity, TDto, TRepository, THandler>(
       Func<IUnitOfWork, THandler> handlerFactory,
       Func<THandler, CancellationToken, Task<ResponseDto<List<TDto>>>> execute,
       Expression<Func<IUnitOfWork, TRepository>> repoSelector,
       List<TEntity> entities,
       Action<Mock<TRepository>>? setupRepoMock = null
   )
       where TEntity : class, IBaseEntity
       where TDto : class, new()
       where TRepository : class, IRepository<TEntity>
       where THandler : class
    {
        // ایجاد موک UnitOfWork و Repository
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        // تنظیم UnitOfWork برای بازگرداندن موک ریپازیتوری
        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // توجه: متد FindListAsync باید لیستی از موجودیت‌ها برگرداند
        repoMock.Setup(r => r.FindListAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        // اعمال تنظیمات اضافی در صورت نیاز
        setupRepoMock?.Invoke(repoMock);

        // ساخت هندلر با استفاده از UnitOfWork موک‌شده
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, CancellationToken.None);

        // بررسی موفقیت‌آمیز بودن عملیات و غیر null بودن داده برگشتی
        Assert.True(result.is_success);
        Assert.NotNull(result.data);
    }

    /// <summary>
    /// تست حالت شکست هندلر در صورت بروز Exception هنگام فراخوانی ریپازیتوری
    /// این تست اطمینان می‌دهد که هندلر در صورت بروز خطا،
    /// نتیجه‌ای با is_success = false و کد پاسخ 500 برمی‌گرداند.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابع ساخت هندلر</param>
    /// <param name="execute">تابع اجرای هندلر</param>
    /// <param name="repoSelector">اکسپریشن انتخاب ریپازیتوری</param>
    /// <param name="setupRepoMock">تنظیمات اختیاری روی موک ریپازیتوری</param>
    public static async Task TestHandle_FailOnException<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<List<TDto>>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector,
        Action<Mock<TRepository>>? setupRepoMock = null)
        where TEntity : class, IBaseEntity
        where TDto : class, new()
        where TRepository : class, IRepository<TEntity>
        where THandler : class
    {
        // ایجاد موک UnitOfWork و Repository
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        // تنظیم موک ریپازیتوری به گونه‌ای که هنگام فراخوانی FindListAsync استثنا پرتاب کند
        repoMock.Setup(r => r.FindListAsync(null, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

        // تنظیم UnitOfWork برای بازگرداندن ریپازیتوری موک شده
        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // اعمال تنظیمات اضافی در صورت نیاز
        setupRepoMock?.Invoke(repoMock);

        // ساخت هندلر با UnitOfWork موک شده
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, CancellationToken.None);

        // بررسی اینکه نتیجه عملیات ناموفق بوده و کد پاسخ 500 است
        Assert.False(result.is_success);
        Assert.Equal(500, result.response_code);

        // اگر بخواهید پیام خطا را هم بررسی کنید، می‌توانید خط زیر را فعال کنید:
        // Assert.Contains("Test exception", result.message);
    }
}

