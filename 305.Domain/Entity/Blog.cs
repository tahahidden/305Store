using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Blog : BaseEntity
{
    public string description { get; set; }
    public string image { get; set; }
    public string image_alt { get; set; }
    public string blog_text { get; set; }
    public bool show_blog { get; set; }
    public string keywords { get; set; }
    public string meta_description { get; set; }
    public int estimated_read_time { get; set; }

    public long blog_category_id { get; set; }
    public BlogCategory blog_category { get; set; }
}
