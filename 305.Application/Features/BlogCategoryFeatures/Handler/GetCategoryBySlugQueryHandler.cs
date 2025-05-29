using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class GetCategoryBySlugQueryHandler : IRequestHandler<GetCategoryBySlugQuery, ResponseDto<BlogCategoryResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly GetBySlugHandler _handler;

	public GetCategoryBySlugQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new GetBySlugHandler(unitOfWork);
	}

	public async Task<ResponseDto<BlogCategoryResponse>> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
	{
		return await _handler.Handle<BlogCategory, BlogCategoryResponse>(
			async uow => await uow.BlogCategoryRepository.FindSingle(x => x.slug == request.slug),
			"دسته‌بندی",
			null
		);
	}
}
