using _305.Application.Base.Response;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class GetBySlugHandlerTestHelper
{
    /// <summary>
    /// تست موفقیت‌آمیز دریافت موجودیت با استفاده از slug
    /// این متد با ساخت موک UnitOfWork و Repository، 
    /// متد FindSingle را برای دریافت موجودیت مشخص تنظیم و اجرای هندلر را بررسی می‌کند.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت که IBaseEntity را پیاده‌سازی کرده</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری که IRepository<TEntity> است</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابعی برای ساخت هندلر با ورودی UnitOfWork</param>
    /// <param name="execute">تابعی که هندلر و توکن لغو می‌گیرد و نتیجه اجرای هندلر را برمی‌گرداند</param>
    /// <param name="repoSelector">اکسپریشن برای انتخاب ریپازیتوری از UnitOfWork</param>
    /// <param name="entity">موجودیت نمونه‌ای که باید توسط موک ریپازیتوری برگردانده شود</param>
    /// <param name="includes">آرایه رشته‌ای از includes اختیاری که باید هنگام فراخوانی FindSingle استفاده شود</param>
    public static async Task TestGetBySlug_Success<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector,
        TEntity entity,
        string[]? includes = null
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

        // اگر includes ارسال نشده بود، آرایه خالی استفاده می‌شود
        var includesToUse = includes ?? Array.Empty<string>();

        // تنظیم FindSingle برای بازگرداندن موجودیت نمونه
        // همراه با بررسی اینکه پارامتر includes ارسال شده مطابق آرایه includesToUse است
        repoMock.Setup(r =>
            r.FindSingle(
                It.IsAny<Expression<Func<TEntity, bool>>>(),
                It.Is<string[]>(inc => inc.SequenceEqual(includesToUse))
            )
        ).ReturnsAsync(entity);

        // ساخت هندلر با UnitOfWork موک شده
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, CancellationToken.None);

        // بررسی موفقیت‌آمیز بودن عملیات و غیر null بودن داده برگشتی
        Assert.True(result.is_success);
        Assert.NotNull(result.data);
    }


    /// <summary>
    /// تست حالت یافت نشدن موجودیت هنگام جستجو بر اساس slug
    /// این متد مطمئن می‌شود که در صورت عدم یافتن موجودیت،
    /// نتیجه‌ای با is_success = false و داده null بازگردانده شود.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="TDto">نوع DTO خروجی</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابع ساخت هندلر</param>
    /// <param name="execute">تابع اجرای هندلر</param>
    /// <param name="repoSelector">اکسپریشن انتخاب ریپازیتوری</param>
    public static async Task TestGetBySlug_NotFound<TEntity, TDto, TRepository, THandler>(
        Func<IUnitOfWork, THandler> handlerFactory,
        Func<THandler, CancellationToken, Task<ResponseDto<TDto>>> execute,
        Expression<Func<IUnitOfWork, TRepository>> repoSelector
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

        // تنظیم FindSingle برای بازگرداندن null (موجودیت یافت نشد)
        repoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<TEntity, bool>>>())).ReturnsAsync((TEntity?)null);

        // ساخت هندلر با UnitOfWork موک شده
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, CancellationToken.None);

        // بررسی شکست عملیات و null بودن داده برگشتی
        Assert.False(result.is_success);
        Assert.Null(result.data);
    }
}

