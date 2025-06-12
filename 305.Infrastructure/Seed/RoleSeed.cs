using _305.Domain.Entity;

namespace _305.Infrastructure.Seed;

public static class RoleSeed
{
    public static List<Role> All =>
    [
        new()
        {
            id = 1,
            name = "Admin",
            created_at = new DateTime(2025, 1, 1, 12, 0, 0),
            updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
            slug = "Admin_Role"
        },
         new()
        {
            id = 2,
            name = "Customer",
            created_at = new DateTime(2025, 1, 1, 12, 0, 0),
            updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
            slug = "Customer_Role"
        },
        new()
        {
            id = 3,
            name = "MainAdmin",
            created_at = new DateTime(2025, 1, 1, 12, 0, 0),
            updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
            slug = "Main_Admin_Role"
        },
    ];
}