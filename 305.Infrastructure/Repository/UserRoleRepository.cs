using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _305.Infrastructure.Repository;
public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
	public UserRoleRepository(ApplicationDbContext context) : base(context)
	{
		DbContext.Set<UserRole>();
	}
}
