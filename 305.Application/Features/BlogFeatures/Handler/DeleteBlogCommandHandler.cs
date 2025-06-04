using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogFeatures.Handler;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, ResponseDto<string>>
{
	private readonly DeleteHandler _handler;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFileService _fileService;

	public DeleteBlogCommandHandler(IUnitOfWork unitOfWork , IFileService fileService)
	{
		_unitOfWork = unitOfWork;
		_handler = new DeleteHandler(unitOfWork);
		_fileService = fileService;
	}

	public async Task<ResponseDto<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
	{
		return await _handler.HandleAsync<Blog, string>(
			findEntityAsync: () => _unitOfWork.BlogRepository.FindSingle(x => x.id == request.id),
			onBeforeDeleteAsync: entity => _fileService.DeleteImage(entity.image),
			onDeleteAsync: entity => _unitOfWork.BlogRepository.Remove(entity),
			entityName: "مقاله",
			notFoundMessage: "مقاله پیدا نشد",
			successMessage: "مقاله با موفقیت حذف شد",
			cancellationToken: cancellationToken
		);
	}
}