using _305.Application.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Application.IUnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IBlogCategoryRepository BlogCategoryRepository { get; }
    IBlogRepository BlogRepository { get; }
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}
