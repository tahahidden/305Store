using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;
public class BlogRepository : Repository<Blog>, IBlogRepository
{
    private readonly IQueryable<Blog> _queryable;

    public BlogRepository(ApplicationDbContext context) : base(context)
    {
        _queryable = DbContext.Set<Blog>();
    }
}