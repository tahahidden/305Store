using _305.Application.Base.ExtensionAndSort;
using _305.Application.Base.Pagination;
using _305.Application.IBaseRepository;
using _305.Domain.Common;
using _305.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace _305.Infrastructure.BaseRepository;
/// <summary>
/// پیاده‌سازی عمومی اینترفیس IRepository برای کار با DbContext و EF Core.
/// </summary>
/// <typeparam name="TEntity">نوع موجودیت که باید کلاس و IBaseEntity باشد.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly DbContext DbContext;

    /// <summary>
    /// سازنده مخزن که DbContext را دریافت می‌کند.
    /// </summary>
    /// <param name="dbContext">مقدار DbContext تزریق شده</param>
    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    #region Queries

    /// <summary>
    /// بررسی وجود حداقل یک رکورد در جدول
    /// </summary>
    public async Task<bool> ExistsAsync()
    {
        return await DbContext.Set<TEntity>().AnyAsync();
    }

    /// <summary>
    /// بررسی وجود حداقل یک رکورد با شرط مشخص
    /// </summary>
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    #endregion

    #region Sync Commands

    /// <summary>
    /// افزودن یک موجودیت به context (منتظر ذخیره نیست)
    /// </summary>
    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    /// <summary>
    /// افزودن مجموعه‌ای از موجودیت‌ها به context (منتظر ذخیره نیست)
    /// </summary>
    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
    }

    /// <summary>
    /// حذف یک موجودیت از context
    /// </summary>
    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    /// حذف مجموعه‌ای از موجودیت‌ها از context
    /// </summary>
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    /// <summary>
    /// بروزرسانی یک موجودیت در context
    /// </summary>
    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    /// <summary>
    /// بروزرسانی مجموعه‌ای از موجودیت‌ها در context
    /// </summary>
    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);
    }

    #endregion

    #region Async Commands

    /// <summary>
    /// افزودن یک موجودیت به صورت async به context
    /// </summary>
    public async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
    }

    /// <summary>
    /// افزودن مجموعه‌ای از موجودیت‌ها به صورت async به context
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    /// <summary>
    /// افزودن یک موجودیت به صورت async به context و برگرداندن آن
    /// </summary>
    public async Task<TEntity> AddAsyncReturnId(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// شمارش کل رکوردها
    /// </summary>
    public int Count()
    {
        return DbContext.Set<TEntity>().Count();
    }

    /// <summary>
    /// شمارش رکوردها بر اساس شرط
    /// </summary>
    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().Count(predicate);
    }

    /// <summary>
    /// بررسی وجود حداقل یک رکورد مطابق شرط به صورت async
    /// </summary>
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    /// <summary>
    /// اعمال includes روی کوئری برای بارگذاری eager
    /// </summary>
    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, string[]? includes)
    {
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }

    /// <summary>
    /// یافتن یک موجودیت که دقیقا یک رکورد شرط را داشته باشد
    /// </summary>
    public async Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return await query.SingleOrDefaultAsync(predicate);
    }

    /// <summary>
    /// یافتن اولین موجودیت مطابق شرط
    /// </summary>
    public async Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return await query.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// یافتن لیست موجودیت‌ها مطابق شرط به صورت همزمان (sync)
    /// </summary>
    public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return query.Where(predicate).ToList();
    }

    /// <summary>
    /// یافتن همه رکوردها بدون شرط
    /// </summary>
    public List<TEntity> FindList(params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return query.ToList();
    }

    /// <summary>
    /// یافتن یک موجودیت به صورت AsNoTracking (بدون ردیابی) که دقیقا یک رکورد شرط را داشته باشد
    /// </summary>
    public async Task<TEntity?> FindSingleAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return await query.SingleOrDefaultAsync(predicate);
    }

    /// <summary>
    /// یافتن اولین موجودیت به صورت AsNoTracking مطابق شرط
    /// </summary>
    public async Task<TEntity?> FindFirstAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return await query.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// یافتن لیست موجودیت‌ها مطابق شرط به صورت AsNoTracking
    /// </summary>
    public List<TEntity> FindListAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return query.Where(predicate).ToList();
    }

    /// <summary>
    /// دریافت نتایج صفحه بندی شده مطابق فیلتر ورودی، با قابلیت شامل کردن Includes
    /// </summary>
    public async Task<PaginatedList<TEntity>> GetPagedResultAsync(DefaultPaginationFilter filter, Expression<Func<TEntity, bool>>? predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        if (predicate != null)
            query = query.Where(predicate);

        if (typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var filterableQuery = query.Cast<IBaseEntity>();
            filterableQuery = filterableQuery.ApplyFilter(filter);
            filterableQuery = filterableQuery.ApplySort(filter.SortBy);
            query = filterableQuery.Cast<TEntity>();
        }
        else
        {
            query = query.OrderByDescending(x => EF.Property<object>(x, "Id"));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginatedList<TEntity>(items, totalCount, filter.Page, filter.PageSize);
    }

    #endregion
}
