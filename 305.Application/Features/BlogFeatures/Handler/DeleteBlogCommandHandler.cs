using _304.Net.Platform.Application.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogFeatures.Command;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _305.Application.Features.BlogFeatures.Handler;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBlogCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new DeleteHandler(unitOfWork);
    }

    public async Task<ResponseDto<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<Blog, string>(
            findEntity: () => _unitOfWork.BlogRepository.FindSingle(x => x.id == request.id),
            onDelete: entity => _unitOfWork.BlogRepository.Remove(entity),
            name: "مقاله",
            notFoundMessage: "مقاله پیدا نشد",
            successMessage: "مقاله با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}