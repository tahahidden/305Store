using _305.BuildingBlocks.Enums;

namespace _305.Application.Filters.Pagination;

// کلاس فیلتر صفحه‌بندی عمومی با تنظیمات پیش‌فرض و محدودیت‌ها
public class PaginationFilter
{
    private const int MinPageNumber = 1;      // حداقل شماره صفحه معتبر
    private const int MaxPageSize = 200;      // حداکثر اندازه صفحه معتبر

    // سازنده با مقادیر پیش‌فرض برای شماره صفحه، اندازه صفحه و ترتیب مرتب‌سازی
    public PaginationFilter(int pageNumber = MinPageNumber, int pageSize = 10, SortByEnum sortBy = SortByEnum.created_at)
    {
        // بررسی و تنظیم مقدار شماره صفحه (باید بزرگتر از صفر باشد)
        Page = pageNumber > 0 ? pageNumber : MinPageNumber;

        // بررسی و تنظیم اندازه صفحه (بین 1 تا MaxPageSize)
        PageSize = pageSize > 0 && pageSize <= MaxPageSize ? pageSize : MaxPageSize;

        // تنظیم نوع مرتب‌سازی
        SortByEnum = sortBy;
    }

    public int Page { get; set; }            // شماره صفحه جاری
    public int PageSize { get; set; }        // تعداد آیتم‌ها در هر صفحه
    public int TotalPageCount { get; set; }  // تعداد کل صفحات (برای اطلاع/نمایش)
    public SortByEnum SortByEnum { get; set; }  // نحوه مرتب‌سازی داده‌ها
}
