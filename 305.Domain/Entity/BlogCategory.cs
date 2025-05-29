using _305.Domain.Common;

namespace _305.Domain.Entity;
public class BlogCategory : BaseEntity
{
	public BlogCategory()
	{
		blogs = new List<Blog>(); // مقداردهی اولیه برای جلوگیری از null reference exception
	}
	public string? description { get; set; }

	public ICollection<Blog> blogs { get; set; }
}
