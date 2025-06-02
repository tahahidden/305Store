using _305.Application.Base.Pagination;
using _305.Domain.Common;
using System.Linq.Expressions;

namespace _305.Application.IBaseRepository;
/// <summary>
/// اینترفیس مخزن داده عمومی برای عملیات پایه‌ای روی موجودیت‌ها.
/// </summary>
/// <typeparam name="TEntity">نوع موجودیت که باید کلاس و IBaseEntity باشد.</typeparam>
public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<bool> ExistsAsync();
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    int Count();
    int Count(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> AddAsyncReturnId(TEntity entity);

    Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindList(params string[]? includes);

    Task<TEntity?> FindSingleAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    Task<TEntity?> FindFirstAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindListAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);

    Task<PaginatedList<TEntity>> GetPagedResultAsync(DefaultPaginationFilter filter, Expression<Func<TEntity, bool>>? predicate, params string[]? includes);
}
