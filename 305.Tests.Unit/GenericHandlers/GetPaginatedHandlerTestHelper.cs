using _305.Application.Base.Response;
using _305.Application.Filters.Pagination;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.GenericHandlers;
public static class GetPaginatedHandlerTestHelper
{
    /// <summary>
    /// تست موفقیت‌آمیز دریافت داده‌های صفحه‌بندی شده (Paginated)
    /// این متد با موک کردن UnitOfWork و Repository، متد GetPagedResultAsync را تنظیم کرده
    /// و اجرای هندلر را بررسی می‌کند که داده‌های صفحه‌بندی شده مطابق انتظار باشد.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت که IBaseEntity را پیاده‌سازی کرده</typeparam>
    /// <typeparam name="TRepository">نوع ریپازیتوری که IRepository<TEntity> است</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <typeparam name="TQuery">نوع کوئری که شامل پارامترهای صفحه‌بندی است</typeparam>
    /// <param name="handlerFactory">تابعی برای ساخت هندلر با ورودی UnitOfWork</param>
    /// <param name="execute">تابعی که هندلر، کوئری و توکن لغو می‌گیرد و نتیجه اجرای هندلر را برمی‌گرداند</param>
    /// <param name="repoSelector">اکسپریشن برای انتخاب ریپازیتوری از UnitOfWork</param>
    /// <param name="query">پارامترهای کوئری برای صفحه‌بندی</param>
    /// <param name="expectedList">لیست صفحه‌بندی شده مورد انتظار برای مقایسه</param>
    /// <param name="includes">آرایه رشته‌ای از includes اختیاری که هنگام فراخوانی GetPagedResultAsync ارسال می‌شود</param>
    public static async Task TestPaginated_Success<TEntity, TRepository, THandler, TQuery>(
    Func<IUnitOfWork, THandler> handlerFactory,
    Func<THandler, TQuery, CancellationToken, Task<ResponseDto<PaginatedList<TEntity>>>> execute,
    Expression<Func<IUnitOfWork, TRepository>> repoSelector,
    TQuery query,
    PaginatedList<TEntity> expectedList
)
    where TEntity : class, IBaseEntity
    where TRepository : class, IRepository<TEntity>
    where THandler : class
    {
        // ساخت موک UnitOfWork و Repository
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();

        unitOfWorkMock.Setup(repoSelector).Returns(repoMock.Object);

        // ستاپ متد GetPagedResultAsync با includeFunc به عنوان پارامتر
        repoMock.Setup(r =>
            r.GetPagedResultAsync(
                It.IsAny<DefaultPaginationFilter>(),
                It.IsAny<Expression<Func<TEntity, bool>>>(),
                It.IsAny<Func<IQueryable<TEntity>, IQueryable<TEntity>>>()
            )
        ).ReturnsAsync(expectedList);

        // ساخت هندلر
        var handler = handlerFactory(unitOfWorkMock.Object);

        // اجرای هندلر
        var result = await execute(handler, query, CancellationToken.None);

        // بررسی موفقیت‌آمیز بودن پاسخ
        Assert.True(result.is_success);
        Assert.NotNull(result.data);
        Assert.Equal(expectedList.Data.Count, result.data.Data.Count);
    }
}



