using _305.Application.IBaseRepository;
using _305.Domain.Entity;

namespace _305.Application.IUOW;
public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    IRepository<BlogCategory> BlogCategoryRepository { get; }
    IRepository<Blog> BlogRepository { get; }
    IRepository<BlacklistedToken> TokenBlacklistRepository { get; }
    IRepository<Permission> PermissionRepository { get; }
    IRepository<RolePermission> RolePermissionRepository { get; }
    IRepository<Role> RoleRepository { get; }
    IRepository<User> UserRepository { get; }
    IRepository<UserRole> UserRoleRepository { get; }
    IRepository<ProductCategory> ProductCategoryRepository { get; }
    IRepository<Product> ProductRepository { get; }
    IRepository<Domain.Entity.Attribute> AttributeRepository { get; }
    IRepository<AttributeOption> AttributeOptionRepository { get; }
    IRepository<ProductAttributeOptionValue> ProductAttributeOptionValueRepository { get; }



    Task<bool> CommitAsync(CancellationToken cancellationToken);
}
