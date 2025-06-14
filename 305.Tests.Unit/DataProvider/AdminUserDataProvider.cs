using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.BuildingBlocks.Security;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class AdminUserDataProvider
{
    public static CreateAdminUserCommand Create(string name = "name", string slug = "slug", string email = "info@305.com")
=> new()
{
    name = name,
    created_at = DateTime.UtcNow,
    slug = slug,
    updated_at = DateTime.UtcNow,
    email = email + Guid.NewGuid(),
    password = "QAZqaz!@#123",
};

    public static EditAdminUserCommand Edit(string name = "name", long id = 1, string slug = "slug")
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
            email = "info@304.com" + Guid.NewGuid(),
            password = "QAZqaz!@#123",
        };


    public static User Row(string name = "name", long id = 1,
        string slug = "slug", int failedLoginCount = 0,
        bool isLockedOut = false)
    => new()
    {
        id = id,
        email = "info@304.com",
        failed_login_count = failedLoginCount,
        is_locked_out = isLockedOut,
        is_delete_able = false,
        mobile = "09309309393",
        name = name,
        password_hash = PasswordHasher.Hash("QAZqaz!@#123"), // QAZqaz!@#123
        concurrency_stamp = "X3JO2EOCURAEBU6HHY6OBYEDD2877FXU",
        security_stamp = "098NTB7E5LFFXREHBSEHDKLI0DOBIKST",
        created_at = new DateTime(2025, 1, 1, 12, 0, 0),
        updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
        slug = slug,
        is_active = true,
        is_mobile_confirmed = true,
        last_login_date_time = DateTime.Now,
        lock_out_end_time = DateTime.Now,
        refresh_token = "refresh_token",
        refresh_token_expiry_time = DateTime.Now.AddDays(15),
    };

    public static DeleteAdminUserCommand Delete(long id = 1)
        => new()
        {
            id = id,
        };

    public static GetAdminUserBySlugQuery GetBySlug(string slug = "slug")
    => new()
    {
        slug = slug,
    };

    public static GetPaginatedAdminUserQuery GetByQueryFilter(string searchTerm = "")
    => new()
    {
        Page = 1,
        PageSize = 10,
        SearchTerm = searchTerm,
    };

    public static PaginatedList<User> GetPaginatedList()
        => PaginatedListFactory.Create(new List<User>
        {
            Row(name: "Tech", id: 1, slug: "tech"),
            Row(name: "Health", id: 2, slug: "health")
        });


    public static AdminLoginCommand LoginCommand(string email = "info@304.com", string password = "QAZqaz!@#123")
        => new()
        {
            email = email,
            password = password
        };
}
