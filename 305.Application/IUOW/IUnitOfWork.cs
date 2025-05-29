using _305.Application.IRepository;

namespace _305.Application.IUOW;
public interface IUnitOfWork : IDisposable
{
	IBlogCategoryRepository BlogCategoryRepository { get; }
	IBlogRepository BlogRepository { get; }
	Task<bool> CommitAsync(CancellationToken cancellationToken);
}
