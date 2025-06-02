namespace _305.BuildingBlocks.Pagination;

// کلاس لیست صفحه‌بندی شده برای نتایج قابل صفحه‌بندی
public class PaginatedList<T>
{
    public int Page { get; set; }               // صفحه فعلی (برای سازگاری با فیلترهای قبلی)
    public int PageSize { get; set; }           // تعداد آیتم‌ها در هر صفحه
    public int TotalCount { get; set; }         // تعداد کل آیتم‌ها در کل مجموعه
    public List<T> Data { get; set; }           // داده‌های صفحه فعلی

    public int CurrentPage { get; set; } = 1;   // شماره صفحه فعلی
    public int TotalPages { get; set; }         // تعداد کل صفحات

    // آیا صفحه قبلی وجود دارد؟
    public bool HasPrevious => CurrentPage > 1;

    // آیا صفحه بعدی وجود دارد؟
    public bool HasNext => CurrentPage < TotalPages;

    // سازنده که داده‌ها، تعداد کل، شماره صفحه و اندازه صفحه را می‌گیرد
    public PaginatedList(IEnumerable<T> items, int count, int page, int pageSize)
    {
        TotalCount = count;

        // اطمینان از اینکه اندازه صفحه مثبت است (جلوگیری از تقسیم بر صفر)
        PageSize = pageSize > 0 ? pageSize : 10;

        // اطمینان از اینکه شماره صفحه حداقل 1 است
        CurrentPage = page > 0 ? page : 1;

        // مقدار Page را برابر CurrentPage تنظیم می‌کنیم (سازگاری)
        Page = CurrentPage;

        // محاسبه تعداد کل صفحات با گرد کردن به بالا
        TotalPages = TotalCount > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        // تبدیل داده‌های ورودی به لیست، یا مقداردهی لیست خالی در صورت null بودن
        Data = items?.ToList() ?? new List<T>();
    }
}
