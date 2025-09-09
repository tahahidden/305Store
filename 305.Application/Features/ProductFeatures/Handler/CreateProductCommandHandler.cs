using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.ProductFeatures.Command;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.ProductFeatures.Handler
{
    public class CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductCategoryService categoryService, bool isTestEnvironment = false)
   : IRequestHandler<CreateProductCommand, ResponseDto<string>>
    {
        private readonly CreateHandler _handler = new(unitOfWork);
        private readonly IProductCategoryService _categoryService = categoryService;
        public async Task<ResponseDto<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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
               var entity = Mapper.Map<CreateProductCommand, Product>(request);
               await unitOfWork.ProductRepository.AddAsync(entity);
               if (request.productCategoryIds != null && request.productCategoryIds.Any())
               {
                   await _categoryService.AddCategoryRelations(entity.id, request.productCategoryIds);
               }
               return slug;
           },
           successMessage: null,
           cancellationToken: cancellationToken
       );
        }
    }
}