using _305.Application.IUOW;
using _305.Infrastructure.Persistence;
using _305.Tests.Integration.Base.Factory;
using _305.WebApi.Assistants.Permission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace _305.Tests.Integration.Base;
[TestFixture]
public class SyncPermissionsTest
{
    private CustomWebApplicationFactory _factory;
    private IServiceScope _scope;
    private IUnitOfWork _unitOfWork;

    [SetUp]
    public async Task SetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _scope = _factory.Services.CreateScope();
        _unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var db = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureDeletedAsync(); // پاک‌سازی دیتابیس
        await db.Database.MigrateAsync();       // اعمال مایگریشن برای ایجاد جدول Permission

        var seeder = new PermissionSeeder(_unitOfWork);
        await seeder.SyncPermissionsAsync();    // اجرای سیدر فقط اینجا

        await _unitOfWork.CommitAsync(CancellationToken.None);
    }



    [TearDown]
    public void TearDown()
    {
        _scope.Dispose();
        _factory.Dispose();
    }
    [Test]
    public void Should_Seed_Permissions_Correctly()
    {
        // Act
        var permissions = _unitOfWork.PermissionRepository.FindList();

        // Assert
        Assert.That(permissions, Is.Not.Empty);
        Assert.That(permissions.Any(p => p.slug == "AdminAuth.Login"),
            "Expected permission with slug 'AdminAuth.Login' not found.");
    }

    //[Test]
    //public async Task Should_Seed_Permissions_Correctly()
    //{
    //	// Arrange
    //	var seeder = new PermissionSeeder(_unitOfWork);

    //	// Act
    //	await seeder.SyncPermissionsAsync();

    //	// Assert
    //	var permissions = _unitOfWork.PermissionRepository.FindList();

    //	// حداقل باید یکی باشه
    //	Assert.That(permissions, Is.Not.Empty);

    //	// به‌صورت اختیاری بررسی دقیق‌تر:
    //	Assert.That(permissions.Any(p => p.name.Contains("AdminAuth.Login") || p.slug.Contains("AdminAuth.Login")),
    //		"Expected permission with name or slug containing 'AdminAuth.Login' not found.");
    //}
}
