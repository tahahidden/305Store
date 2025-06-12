using _305.BuildingBlocks.Enums;

namespace _305.Application.Filters.Pagination;

// کلاس فیلتر صفحه‌بندی عمومی با تنظیمات پیش‌فرض و محدودیت‌ها
public class PaginationFilter(
    int pageNumber = PaginationFilter.MinPageNumber,
    int pageSize = 10,
    SortByEnum sortBy = SortByEnum.created_at)
{
    private const int MinPageNumber = 1;      // حداقل شماره صفحه معتبر
    private const int MaxPageSize = 200;      // حداکثر اندازه صفحه معتبر

    public int Page { get; set; } = pageNumber > 0 ? pageNumber : MinPageNumber; // شماره صفحه جاری
    public int PageSize { get; set; } = pageSize is > 0 and <= MaxPageSize ? pageSize : MaxPageSize; // تعداد آیتم‌ها در هر صفحه
    public int TotalPageCount { get; set; }  // تعداد کل صفحات (برای اطلاع/نمایش)
    public SortByEnum SortByEnum { get; set; } = sortBy; // نحوه مرتب‌سازی داده‌ها
}
