using System.Linq.Expressions;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using Moq;

namespace _305.Tests.Unit.Assistant;

/// <summary>
/// Provides factory methods for creating mocked repositories
/// and wiring them with <see cref="IUnitOfWork"/>.
/// </summary>
public static class RepositoryMockFactory
{
    /// <summary>
    /// Creates mocks for <see cref="IUnitOfWork"/> and a repository
    /// and sets up the repository property on the unit of work.
    /// </summary>
    public static (Mock<IUnitOfWork> uowMock, Mock<TRepository> repoMock) CreateFor<TRepository>(
        Expression<Func<IUnitOfWork, TRepository>> repoSelector)
        where TRepository : class
    {
        var uowMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<TRepository>();
        uowMock.Setup(repoSelector).Returns(repoMock.Object);
        return (uowMock, repoMock);
    }
}
