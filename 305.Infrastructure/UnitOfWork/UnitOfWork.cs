using _305.Application.IRepository;
using _305.Application.IUOW;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.Repository;

namespace _305.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _context;
	public UnitOfWork(ApplicationDbContext context)
	{
		_context = context;
		BlogCategoryRepository = new BlogCategoryRepository(_context);
		BlogRepository = new BlogRepository(_context);
		TokenBlacklistRepository = new TokenBlacklistRepository(_context);
		PermissionRepository = new PermissionRepository(_context);
		RolePermissionRepository = new RolePermissionRepository(_context);
		RoleRepository = new RoleRepository(_context);
		UserRepository = new UserRepository(_context);
		UserRoleRepository = new UserRoleRepository(_context);
	}

	public IBlogCategoryRepository BlogCategoryRepository { get; }
	public IBlogRepository BlogRepository { get; }
	public ITokenBlacklistRepository TokenBlacklistRepository { get; set; }
	public IPermissionRepository PermissionRepository { get; set; }
	public IRolePermissionRepository RolePermissionRepository { get; set; }
	public IRoleRepository RoleRepository { get; set; }
	public IUserRepository UserRepository { get; set; }
	public IUserRoleRepository UserRoleRepository { get; set; }

	/// <summary>
	/// تلاش برای ذخیره‌سازی تمامی تغییرات در پایگاه داده در قالب یک تراکنش.
	/// در صورت بروز خطا، تراکنش برگشت داده می‌شود (Rollback).
	/// </summary>
	/// <param name="cancellationToken">توکن لغو عملیات برای توقف عملیات به صورت ناهمگام.</param>
	/// <returns>اگر ذخیره‌سازی با موفقیت انجام شود، مقدار true بازمی‌گرداند؛ در غیر این صورت، false.</returns>
	public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
	{
		// شروع یک تراکنش جدید در پایگاه داده
		await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

		try
		{
			// ذخیره‌سازی تمام تغییرات انجام‌شده در Context در پایگاه داده
			await _context.SaveChangesAsync(cancellationToken);

			// در صورت موفقیت‌آمیز بودن ذخیره‌سازی، تراکنش را تأیید کن (Commit)
			await transaction.CommitAsync(cancellationToken);

			return true;
		}
		catch
		{
			// در صورت وقوع خطا، تراکنش را به حالت اولیه برگردان (Rollback)
			await transaction.RollbackAsync(cancellationToken);

			// پرتاب مجدد استثنا برای رسیدگی در لایه بالاتر
			throw;
		}
	}



	// dispose and add to garbage collector
	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}