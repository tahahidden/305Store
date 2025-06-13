using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Blog : BaseEntity
{
    public string description { get; set; } = null!;
    public string image { get; set; } = null!;
    public string image_alt { get; set; } = null!;
    public string blog_text { get; set; } = null!;
    public bool show_blog { get; set; }
    public string keywords { get; set; } = null!;

    public string meta_description { get; set; } = null!;
    public int estimated_read_time { get; set; }

    public long blog_category_id { get; set; }
    public BlogCategory? blog_category { get; set; }

    /// <summary>
    /// سازنده‌ای برای ایجاد وبلاگ با حداقل مقادیر لازم
    /// </summary>
    public Blog(string name, string slug, string description, string image, string image_alt, string blog_text, string keywords, string meta_description, int estimated_read_time, long blog_category_id)
        : base(name, slug)
    {
        this.description = description;
        this.image = image;
        this.image_alt = image_alt;
        this.blog_text = blog_text;
        this.keywords = keywords;
        this.meta_description = meta_description;
        this.estimated_read_time = estimated_read_time;
        this.blog_category_id = blog_category_id;
    }

    public Blog() { }
}
