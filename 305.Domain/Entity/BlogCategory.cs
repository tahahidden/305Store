using _305.Domain.Common;

namespace _305.Domain.Entity;
public class BlogCategory : BaseEntity
{
    public string? description { get; set; }

    public ICollection<Blog>? blogs { get; set; }

    /// <summary>
    /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
    /// </summary>
    public BlogCategory(string name, string slug, string? description = null) : base(name, slug)
    {
        this.description = description;
    }

    public BlogCategory() { }
}
