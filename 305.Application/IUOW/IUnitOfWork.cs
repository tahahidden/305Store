using _305.Application.IRepository;

namespace _305.Application.IUOW;
public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
	IBlogCategoryRepository BlogCategoryRepository { get; }
	IBlogRepository BlogRepository { get; }
	ITokenBlacklistRepository TokenBlacklistRepository { get; }
	IPermissionRepository PermissionRepository { get; }
	IRolePermissionRepository RolePermissionRepository { get; }
	IRoleRepository RoleRepository { get; }
	IUserRepository UserRepository { get; }
	IUserRoleRepository UserRoleRepository { get; }
	Task<bool> CommitAsync(CancellationToken cancellationToken);
}
