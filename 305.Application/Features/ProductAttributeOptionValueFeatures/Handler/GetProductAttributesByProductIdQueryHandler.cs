using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Query;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Response;
using _305.Application.IUOW;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.ProductAttributeOptionValueFeatures.Handler
{
    public class GetProductAttributeOptionValuesByProductIdQueryHandler : IRequestHandler<GetAttrValuesByProductIdQuery, ResponseDto<List<ProductAttributeOptionValueResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetAllHandler _handler = new();

        public GetProductAttributeOptionValuesByProductIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<List<ProductAttributeOptionValueResponse>>> Handle(GetAttrValuesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.ProductAttributeOptionValueRepository.FindListAsync(
                predicate: x => x.productAttribute != null && x.productAttribute.productId == request.productId,
                includeFunc: q => q
                    .Include(x => x.productAttribute)
                    .Include(x => x.attributeOption),
                cancellationToken: cancellationToken
            );

            return await _handler.HandleAsync<Domain.Entity.ProductAttributeOptionValue, ProductAttributeOptionValueResponse>(query);
        }
    }
}