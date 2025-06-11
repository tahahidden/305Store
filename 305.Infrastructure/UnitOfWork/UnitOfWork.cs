using _305.Application.IRepository;
using _305.Application.IUOW;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.Repository;

namespace _305.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
	
	private readonly ApplicationDbContext _context;

	//Lazy Initialization * (توضیحاتت پایین صفحه)
	private readonly Lazy<IBlogCategoryRepository> _blogCategoryRepository;
	private readonly Lazy<IBlogRepository> _blogRepository;
	private readonly Lazy<ITokenBlacklistRepository> _tokenBlacklistRepository;
	private readonly Lazy<IPermissionRepository> _permissionRepository;
	private readonly Lazy<IRolePermissionRepository> _rolePermissionRepository;
	private readonly Lazy<IRoleRepository> _roleRepository;
	private readonly Lazy<IUserRepository> _userRepository;
	private readonly Lazy<IUserRoleRepository> _userRoleRepository;

	public UnitOfWork(ApplicationDbContext context)
	{
		_context = context;

		_blogCategoryRepository = new Lazy<IBlogCategoryRepository>(() => new BlogCategoryRepository(_context));
		_blogRepository = new Lazy<IBlogRepository>(() => new BlogRepository(_context));
		_tokenBlacklistRepository = new Lazy<ITokenBlacklistRepository>(() => new TokenBlacklistRepository(_context));
		_permissionRepository = new Lazy<IPermissionRepository>(() => new PermissionRepository(_context));
		_rolePermissionRepository = new Lazy<IRolePermissionRepository>(() => new RolePermissionRepository(_context));
		_roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(_context));
		_userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
		_userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(_context));
	}

	// Properties که فقط مقدار Lazy.Value رو برمی‌گردونن
	public IBlogCategoryRepository BlogCategoryRepository => _blogCategoryRepository.Value;
	public IBlogRepository BlogRepository => _blogRepository.Value;
	public ITokenBlacklistRepository TokenBlacklistRepository => _tokenBlacklistRepository.Value;
	public IPermissionRepository PermissionRepository => _permissionRepository.Value;
	public IRolePermissionRepository RolePermissionRepository => _rolePermissionRepository.Value;
	public IRoleRepository RoleRepository => _roleRepository.Value;
	public IUserRepository UserRepository => _userRepository.Value;
	public IUserRoleRepository UserRoleRepository => _userRoleRepository.Value;

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
	public async ValueTask DisposeAsync()
	{
		await _context.DisposeAsync();
		GC.SuppressFinalize(this);
	}

	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}

// note: Lazy Initialization :

//Lazy Initialization چیست؟
//وقتی یک شیء(Object) را در برنامه می‌سازیم، بعضی وقت‌ها ممکنه ساختن اون شیء زمان‌بر یا هزینه‌بر باشه، یا اصلاً شاید در یک اجرای خاص اصلاً لازم نباشه اون شیء ساخته بشه.

//	Lazy Initialization یعنی:
//ساخت و مقداردهی شیء را تا زمانی که واقعاً نیاز باشد به تعویق بندازیم. یعنی وقتی کد خواست از اون شیء استفاده کنه، تازه اون رو بسازیم.

//	چرا Lazy Initialization خوب است؟
//صرفه‌جویی در منابع و زمان
//	اگر هر بار UnitOfWork رو ساختیم همه ریپازیتوری‌ها را بسازیم، حتی آنهایی که استفاده نمی‌شوند، باعث استفاده بی‌مورد از حافظه و زمان می‌شود.

//	کاهش بار شروع برنامه
//شروع سریع‌تر چون فقط وقتی یک ریپازیتوری واقعاً لازم شد ساخته می‌شود.

//	کاهش وابستگی‌ها و بهبود انعطاف‌پذیری
//	فقط اشیاء ضروری ساخته می‌شوند و خطاهای احتمالی ساخت سایر اشیاء به تأخیر می‌افتد.

