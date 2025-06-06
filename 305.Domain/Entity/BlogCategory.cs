using _305.Domain.Common;

namespace _305.Domain.Entity;
public class BlogCategory : BaseEntity
{
    public string? description { get; set; }

    public ICollection<Blog>? blogs { get; set; }
}
