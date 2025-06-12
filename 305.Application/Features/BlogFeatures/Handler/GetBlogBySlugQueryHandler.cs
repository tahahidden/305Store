using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Features.BlogFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.BlogFeatures.Handler;

public class GetBlogBySlugQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetBlogBySlugQuery, ResponseDto<BlogResponse>>
{
    private readonly GetBySlugHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<BlogResponse>> Handle(GetBlogBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _handler.Handle<Blog, BlogResponse>(
            async uow => await uow.BlogRepository.FindSingle(x => x.slug == request.slug,
            q => q.Include(x => x.blog_category)),
            "مقاله",
            null
        );
    }
}