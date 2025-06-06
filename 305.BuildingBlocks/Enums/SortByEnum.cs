namespace _305.BuildingBlocks.Enums;

/// <summary>
/// این enum مشخص می‌کند که مرتب‌سازی بر اساس کدام فیلد و به چه ترتیب انجام شود
/// </summary>
public enum SortByEnum
{
    /// <summary>مرتب‌سازی بر اساس تاریخ ایجاد به صورت صعودی (قدیمی‌ترین تا جدیدترین)</summary>
    created_at = 1,

    /// <summary>مرتب‌سازی بر اساس تاریخ ایجاد به صورت نزولی (جدیدترین تا قدیمی‌ترین)</summary>
    created_at_descending = 2,

    /// <summary>مرتب‌سازی بر اساس تاریخ به‌روزرسانی به صورت صعودی</summary>
    updated_at = 3,

    /// <summary>مرتب‌سازی بر اساس تاریخ به‌روزرسانی به صورت نزولی</summary>
    updated_at_descending = 4,

    /// <summary>مرتب‌سازی بر اساس اسلاگ به صورت صعودی</summary>
    slug = 5,

    /// <summary>مرتب‌سازی بر اساس اسلاگ به صورت نزولی</summary>
    slug_descending = 6,

    /// <summary>مرتب‌سازی بر اساس نام به صورت صعودی</summary>
    name = 7,

    /// <summary>مرتب‌سازی بر اساس نام به صورت نزولی</summary>
    name_descending = 8,
}
