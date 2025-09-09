using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.ProductFeatures.Query;
using _305.Application.Features.ProductFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.ProductFeatures.Handler
{
    public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, ResponseDto<List<ProductResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetAllHandler _handler = new();

        public GetProductsByCategoryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<List<ProductResponse>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.ProductRepository.FindListAsync(
                predicate: x => x.productCategoryId == request.ProductCategoryId,
                includeFunc: q => q.Include(x => x.productCategory),
                cancellationToken: cancellationToken
            );

            return await _handler.HandleAsync<Product, ProductResponse>(query);
        }
    }
}