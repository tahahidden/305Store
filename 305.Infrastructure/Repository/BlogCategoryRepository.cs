using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;
public class BlogCategoryRepository : Repository<BlogCategory>, IBlogCategoryRepository
{
	private readonly IQueryable<BlogCategory> _queryable;

	public BlogCategoryRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<BlogCategory>();
	}
}