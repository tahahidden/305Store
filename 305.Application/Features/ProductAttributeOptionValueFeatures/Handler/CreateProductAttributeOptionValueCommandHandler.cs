using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;

namespace _305.Application.Features.ProductAttributeOptionValueFeatures.Handler
{
    public class CreateProductAttributeOptionValueCommandHandler(IUnitOfWork unitOfWork, bool isTestEnvironment = false)
   : MediatR.IRequestHandler<CreateProductAttributeOptionValueCommand, ResponseDto<string>>
    {
        private readonly CreateHandler _handler = new(unitOfWork);

        public async Task<ResponseDto<string>> Handle(CreateProductAttributeOptionValueCommand request, CancellationToken cancellationToken)
        {
            var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
            var validations = new List<ValidationItem>
        {
           new ()
           {
               Rule = async () => await unitOfWork.ProductRepository.ExistsAsync(x => x.name == request.name),
               Value = "نام"
           },
           new ()
           {
               Rule = async () => await unitOfWork.ProductRepository.ExistsAsync(x => x.slug == slug),
               Value = "نامک"
           }
        };
            return await _handler.HandleAsync(
           validations: validations,
           onCreate: async () =>
           {
               var entity = Mapper.Map<CreateProductAttributeOptionValueCommand, ProductAttributeOptionValue>(request);
               await unitOfWork.ProductAttributeOptionValueRepository.AddAsync(entity);
               return slug;
           },
           successMessage: null,
           cancellationToken: cancellationToken
       );
        }
    }
}