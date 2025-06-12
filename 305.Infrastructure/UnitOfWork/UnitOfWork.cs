using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{

    private readonly ApplicationDbContext _context;

    //Lazy Initialization * (توضیحات پایین صفحه)
    private readonly Lazy<IRepository<BlogCategory>> _blogCategoryRepository;
    private readonly Lazy<IRepository<Blog>> _blogRepository;
    private readonly Lazy<IRepository<BlacklistedToken>> _tokenBlacklistRepository;
    private readonly Lazy<IRepository<Permission>> _permissionRepository;
    private readonly Lazy<IRepository<RolePermission>> _rolePermissionRepository;
    private readonly Lazy<IRepository<Role>> _roleRepository;
    private readonly Lazy<IRepository<User>> _userRepository;
    private readonly Lazy<IRepository<UserRole>> _userRoleRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        _blogCategoryRepository = new Lazy<IRepository<BlogCategory>>(() => new Repository<BlogCategory>(_context));
        _blogRepository = new Lazy<IRepository<Blog>>(() => new Repository<Blog>(_context));
        _tokenBlacklistRepository = new Lazy<IRepository<BlacklistedToken>>(() => new Repository<BlacklistedToken>(_context));
        _permissionRepository = new Lazy<IRepository<Permission>>(() => new Repository<Permission>(_context));
        _rolePermissionRepository = new Lazy<IRepository<RolePermission>>(() => new Repository<RolePermission>(_context));
        _roleRepository = new Lazy<IRepository<Role>>(() => new Repository<Role>(_context));
        _userRepository = new Lazy<IRepository<User>>(() => new Repository<User>(_context));
        _userRoleRepository = new Lazy<IRepository<UserRole>>(() => new Repository<UserRole>(_context));
    }

    // Properties که فقط مقدار Lazy.Value رو برمی‌گردونن
    public IRepository<BlogCategory> BlogCategoryRepository => _blogCategoryRepository.Value;
    public IRepository<Blog> BlogRepository => _blogRepository.Value;
    public IRepository<BlacklistedToken> TokenBlacklistRepository => _tokenBlacklistRepository.Value;
    public IRepository<Permission> PermissionRepository => _permissionRepository.Value;
    public IRepository<RolePermission> RolePermissionRepository => _rolePermissionRepository.Value;
    public IRepository<Role> RoleRepository => _roleRepository.Value;
    public IRepository<User> UserRepository => _userRepository.Value;
    public IRepository<UserRole> UserRoleRepository => _userRoleRepository.Value;

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

