using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.ProductCategoryFeatures.Query;
using _305.Application.Features.ProductCategoryFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.ProductCategoryFeatures.Handler
{
   public class GetAllProductCategoryQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProductCategoryQuery, ResponseDto<List<ProductCategoryResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<ProductCategoryResponse>>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<ProductCategory, ProductCategoryResponse>(
            unitOfWork.ProductCategoryRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}
}