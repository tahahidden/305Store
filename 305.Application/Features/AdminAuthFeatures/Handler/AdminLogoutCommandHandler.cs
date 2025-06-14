using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Configurations;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class AdminLogoutCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<AdminLogoutCommand, ResponseDto<string>>
{
    public static readonly JwtConfig Config = new();

    public async Task<ResponseDto<string>> Handle(AdminLogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Responses.NotFound<string>(null, name: "کاربر");
            var user = await unitOfWork.UserRepository.FindSingle(x => x.id == int.Parse(userId));
            if (user == null)
                return Responses.NotFound<string>(null, name: "کاربر");
            var context = httpContextAccessor.HttpContext;
            // بررسی اینکه هدر Authorization وجود داره یا نه
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                // مقدار هدر معمولاً به شکل "Bearer {access_token}" هست
                var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

                if (string.IsNullOrWhiteSpace(token))
                    return Responses.Fail<string>(null, message: "داشتن توکن الزامی است");


                await unitOfWork.TokenBlacklistRepository.AddAsync(new BlacklistedToken
                {
                    token = token,
                    expiry_date = DateTime.Now.AddDays(Config.AccessTokenLifetime.TotalDays),
                    slug = SlugHelper.GenerateSlug(token)
                });

                // حذف کوکی از مرورگر
                CookieHelper.DeleteJwtCookie(context.Response);
                user.refresh_token = null;
                user.refresh_token_expiry_time = DateTime.MinValue;

                unitOfWork.UserRepository.Update(user);
                await unitOfWork.CommitAsync(cancellationToken);

                return Responses.Success<string>(null, message: "با موفقیت خارج شدید.");
            }
            else
            {
                return Responses.Fail<string>(null, message: "توکن در هدر وجود ندارد.", code: 401);
            }
        }
        catch (Exception ex)
        {
            // لاگ‌گیری با Serilog برای ثبت خطاهای غیرمنتظره
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<string>(null, null);
        }
    }
}
