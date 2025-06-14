using _305.Application.Filters.Pagination;

namespace _305.Tests.Unit.DataProvider;

/// <summary>
/// Factory methods for creating <see cref="PaginatedList{T}"/> instances in tests.
/// </summary>
public static class PaginatedListFactory
{
    /// <summary>
    /// Creates a paginated list from the provided items.
    /// </summary>
    public static PaginatedList<T> Create<T>(IEnumerable<T> items, int page = 1, int pageSize = 10)
    {
        var list = items.ToList();
        return new PaginatedList<T>(list, list.Count, page, pageSize);
    }
}
