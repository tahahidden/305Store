using _305.Application.Features.BlogCategoryFeatures.Response;
using Core.EntityFramework.Models;
using DataLayer.Base.Query;

namespace _305.Application.Features.BlogCategoryFeatures.Query;

public class GetAllCategoryQuery: GetAllQuery<BlogCategoryResponse>
{
}
