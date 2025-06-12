using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Configurations;
using _305.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class AdminRefreshCommandHandler(
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<AdminRefreshCommand, ResponseDto<LoginResponse>>
{
    public static readonly JwtConfig Config = new();

    public async Task<ResponseDto<LoginResponse>> Handle(AdminRefreshCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var refreshToken = ReadRefreshToken();
            if (refreshToken != null)
            {
                var user = await unitOfWork.UserRepository.FindSingle(x => x.refresh_token == refreshToken);
                if (user == null || user.refresh_token_expiry_time < DateTime.Now)
                {
                    DeleteRefreshTokenCookie();
                    return Responses.Fail<LoginResponse>(null, message: "توکن نامعتبر است و یا منقضی شده است", code: 401);
                }

                var role = unitOfWork.UserRoleRepository.FindList(x => x.userid == user.id);
                var token = await JwtTokenHelper.GenerateUniqueAccessToken(
                    jwtService,
                    unitOfWork,
                    user,
                    role.Select(x => x.role?.name).ToList());
                return Responses.Success<LoginResponse>(new LoginResponse()
                {
                    access_token = token,
                    expire_in = Config.AccessTokenLifetime.TotalMinutes,
                }, code: 200);
            }
            else
            {
                return Responses.Fail<LoginResponse>(null, message: "توکن یافت نشد", code: 401);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<LoginResponse>(null, null);
        }
    }

    /// <summary>
    /// خواندن رفرش توکن از کوکی
    /// </summary>
    private string? ReadRefreshToken()
    {
        var context = httpContextAccessor.HttpContext;
        return context.Request.Cookies.TryGetValue("jwt", out var token) ? token : null;
    }

    /// <summary>
    /// حذف کوکی مربوط به رفرش توکن
    /// </summary>
    private void DeleteRefreshTokenCookie()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt");
    }
}
