using Core.Base.EF;
using DataLayer.Base.Response;
using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _305.Tests.Unit.GenericHandlers;
public static class EditHandlerTestHelper
{
    /// <summary>
    /// تست موفقیت‌آمیز ویرایش موجودیت
    /// این متد با استفاده از موک‌های Repository و UnitOfWork،
    /// هندلر را ساخته، عملیات ویرایش را اجرا و نتیجه را بررسی می‌کند.
    /// </summary>
    /// <typeparam name="TCommand">نوع دستور ویرایش</typeparam>
    /// <typeparam name="TEntity">نوع موجودیت (باید IBaseEntity را پیاده‌سازی کند)</typeparam>
    /// <typeparam name="THandler">نوع هندلر دستور ویرایش</typeparam>
    /// <param name="handlerFactory">تابعی که هندلر را با دریافت موک‌های Repository و UnitOfWork می‌سازد</param>
    /// <param name="execute">تابعی که هندلر، دستور و توکن لغو را گرفته و نتیجه اجرا را برمی‌گرداند</param>
    /// <param name="command">دستور ویرایش</param>
    /// <param name="entityId">آیدی موجودیتی که باید ویرایش شود</param>
    /// <param name="existingEntity">نمونه موجودیت فعلی که باید تغییر کند</param>
    /// <param name="assertUpdated">اکشنی برای بررسی تغییرات روی موجودیت پس از ویرایش</param>
    public static async Task TestEditSuccess<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId,
        TEntity existingEntity,
        Action<TEntity> assertUpdated
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        // Arrange - آماده‌سازی موک‌های Repository و UnitOfWork
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // تنظیم Repository برای بازگرداندن موجودیت فعلی با آیدی داده شده
        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync(existingEntity);

        // تنظیم UnitOfWork برای بازگرداندن موفقیت‌آمیز Commit
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        // ساخت هندلر با موک‌های آماده شده
        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);

        // Act - اجرای عملیات ویرایش
        var result = await execute(handler, command, CancellationToken.None);

        // Assert - بررسی نتیجه عملیات و اعتبارسنجی‌ها
        Assert.True(result.is_success);
        Assert.Equal(200, result.response_code);
        Assert.NotNull(result.data);

        // بررسی صحت تغییرات روی موجودیت
        assertUpdated(existingEntity);

        // اطمینان از اینکه FindSingle و CommitAsync هرکدام یکبار صدا زده شده‌اند
        repoMock.Verify(r => r.FindSingle(x => x.id == entityId), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    /// <summary>
    /// تست حالت عدم وجود موجودیت برای ویرایش (Not Found)
    /// وقتی موجودیتی با آن آیدی در دیتابیس پیدا نشود،
    /// انتظار می‌رود نتیجه موفق نباشد و کد 404 بازگردد.
    /// </summary>
    /// <typeparam name="TCommand">نوع دستور ویرایش</typeparam>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابع ساخت هندلر</param>
    /// <param name="execute">تابع اجرای دستور</param>
    /// <param name="command">دستور ویرایش</param>
    /// <param name="entityId">آیدی موجودیت</param>
    public static async Task TestEditNotFound<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        // Arrange - آماده‌سازی موک‌ها
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // تنظیم FindSingle به گونه‌ای که هیچ موجودیتی پیدا نشود (برگشت null)
        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync((TEntity?)null);

        // ساخت هندلر
        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);

        // Act - اجرای دستور ویرایش
        var result = await execute(handler, command, CancellationToken.None);

        // Assert - انتظار عدم موفقیت و کد 404
        Assert.False(result.is_success);
        Assert.Equal(404, result.response_code);

        // اطمینان از اینکه CommitAsync هیچوقت فراخوانی نشده (چون موجودیتی نیست برای ویرایش)
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }


    /// <summary>
    /// تست شکست Commit در هنگام ویرایش
    /// وقتی Commit به دلایلی شکست بخورد،
    /// انتظار می‌رود نتیجه عملیات ناموفق باشد و کد 500 بازگردد.
    /// </summary>
    /// <typeparam name="TCommand">نوع دستور ویرایش</typeparam>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="THandler">نوع هندلر</typeparam>
    /// <param name="handlerFactory">تابع ساخت هندلر</param>
    /// <param name="execute">تابع اجرای دستور</param>
    /// <param name="command">دستور ویرایش</param>
    /// <param name="entityId">آیدی موجودیت</param>
    /// <param name="existingEntity">موجودیت فعلی</param>
    public static async Task TestEditCommitFail<TCommand, TEntity, THandler>(
        Func<IRepository<TEntity>, IUnitOfWork, THandler> handlerFactory,
        Func<THandler, TCommand, CancellationToken, Task<ResponseDto<string>>> execute,
        TCommand command,
        long entityId,
        TEntity existingEntity
    )
        where TEntity : class, IBaseEntity
        where THandler : class
    {
        // Arrange - آماده‌سازی موک‌ها
        var repoMock = new Mock<IRepository<TEntity>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // تنظیم موجودیت فعلی
        repoMock.Setup(r => r.FindSingle(x => x.id == entityId))
                .ReturnsAsync(existingEntity);

        // تنظیم شکست Commit
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false); // شکست در Commit

        // ساخت هندلر
        var handler = handlerFactory(repoMock.Object, unitOfWorkMock.Object);

        // Act - اجرای دستور ویرایش
        var result = await execute(handler, command, CancellationToken.None);

        // Assert - بررسی شکست عملیات و کد 500
        Assert.False(result.is_success);
        Assert.Equal(500, result.response_code);

        // بررسی اینکه FindSingle و CommitAsync هر دو یکبار صدا زده شده‌اند
        repoMock.Verify(r => r.FindSingle(x => x.id == entityId), Times.Once);
        unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}


