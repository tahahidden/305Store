using _305.Application.Features.AdminAuthFeatures.Query;
using _305.Application.Features.AdminAuthFeatures.Response;
using MediatR;
using Serilog;
using System.Security.Claims;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Configurations;
using _305.Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class GetUserProfileQueryHandler(
	IUnitOfWork unitOfWork,
	IJwtService jwtService,
	IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetUserProfileQuery, ResponseDto<UserResponse>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	public static readonly JwtConfig Config = new();
	private readonly IJwtService _tokenService = jwtService;
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public async Task<ResponseDto<UserResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Responses.NotFound<UserResponse>(default, name: "کاربر");
			var user = await _unitOfWork.UserRepository.FindSingleAsNoTracking(x => x.id == int.Parse(userId));
			if (user == null)
				return Responses.NotFound<UserResponse>(default, name: "کاربر");
			var data = Mapper.Map<User, UserResponse>(user);
			return Responses.Data<UserResponse>(data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<UserResponse>(default, null);
		}
	}
}
