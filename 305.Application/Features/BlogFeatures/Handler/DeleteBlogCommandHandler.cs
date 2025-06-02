using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogFeatures.Handler;

public class DeleteBlogCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
	: IRequestHandler<DeleteBlogCommand, ResponseDto<string>>
{
	private readonly DeleteHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
	{
		return await _handler.HandleAsync<Blog, string>(
			findEntityAsync: () => unitOfWork.BlogRepository.FindSingle(x => x.id == request.id),
			onBeforeDeleteAsync: entity => fileService.DeleteImage(entity.image),
			onDeleteAsync: entity => unitOfWork.BlogRepository.Remove(entity),
			entityName: "مقاله",
			notFoundMessage: "مقاله پیدا نشد",
			successMessage: "مقاله با موفقیت حذف شد",
			cancellationToken: cancellationToken
		);
	}
}