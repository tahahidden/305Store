using Core.EntityFramework.Models;
using DataLayer.Base.Query;
using GoldAPI.Application.BlogCategoryFeatures.Response;

namespace GoldAPI.Application.BlogCategoryFeatures.Query;

public class GetAllCategoryQuery: GetAllQuery<BlogCategoryResponse>
{
}
