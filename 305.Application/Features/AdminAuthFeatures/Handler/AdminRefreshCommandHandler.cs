using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Response;
using MediatR;
using Serilog;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class AdminRefreshCommandHandler(
	IUnitOfWork unitOfWork,
	IJwtTokenService jwtTokenService,
	IHttpContextAccessor httpContextAccessor
) : IRequestHandler<AdminRefreshCommand, ResponseDto<LoginResponse>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	public static readonly SecurityTokenConfig Config = new();
	private readonly IJwtTokenService _tokenService = jwtTokenService;
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public async Task<ResponseDto<LoginResponse>> Handle(AdminRefreshCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var context = _httpContextAccessor.HttpContext;
			if (context.Request.Cookies.TryGetValue("jwt", out string refreshToken))
			{
				var user = await _unitOfWork.UserRepository.FindSingle(x => x.refresh_token == refreshToken);
				if (user == null || user.refresh_token_expiry_time < DateTime.Now)
				{
					context.Response.Cookies.Delete("jwt");
					return Responses.Fail<LoginResponse>(default, message: "توکن نامعتبر است و یا منقضی شده است", code: 401);
				}

				var role = _unitOfWork.UserRoleRepository.FindList(x => x.user_id == user.id);
				var token = "";
				do
				{
					token = _tokenService.GenerateToken(user, role.Select(x => x.role.title).ToList());
				}
				while (await _unitOfWork.TokenBlacklistRepository.ExistsAsync(x => x.token == token));
				return Responses.Success<LoginResponse>(new LoginResponse()
				{
					access_token = token,
					expire_in = Config.AccessTokenLifetime.TotalMinutes,
				}, code: 200);
			}
			else
			{
				return Responses.Fail<LoginResponse>(default, message: "توکن یافت نشد", code: 401);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
			return Responses.ExceptionFail<LoginResponse>(default, null);
		}
	}
}
