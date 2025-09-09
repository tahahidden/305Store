using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.ProductCategoryFeatures.Query;
using _305.Application.Features.ProductFeatures.Query;
using _305.Application.Features.ProductFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.ProductFeatures.Handler
{
    public class GetAllProductCategoryQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProductQuery, ResponseDto<List<ProductResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<Product, ProductResponse>(
            unitOfWork.ProductRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}
}