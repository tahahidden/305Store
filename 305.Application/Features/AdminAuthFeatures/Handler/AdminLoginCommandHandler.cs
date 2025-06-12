using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Configurations;
using _305.BuildingBlocks.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace _305.Application.Features.AdminAuthFeatures.Handler;

public class AdminLoginCommandHandler(
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<AdminLoginCommand, ResponseDto<LoginResponse>>
{
    public static readonly JwtConfig Config = new();
    public static readonly LockoutConfig lockOutConfig = new();
    public async Task<ResponseDto<LoginResponse>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await unitOfWork.UserRepository.FindSingle(x => x.email == request.email);
            if (user is not { is_active: true })
                return Responses.NotFound<LoginResponse>(null, name: "کاربر");

            // بررسی قفل شدن کاربر
            if (user.is_locked_out || user.lock_out_end_time > DateTime.Now)
                return Responses.Fail<LoginResponse>(null, message: "اکانت شما موقتا قفل شده است", code: 401);
            // بررسی رمز عبور
            if (!PasswordHasher.Check(user.password_hash, request.password))
            {
                user.failed_login_count++;
                if (user.failed_login_count >= lockOutConfig.FailedLoginLimit)
                {
                    user.is_locked_out = true;
                    user.lock_out_end_time = DateTime.Now.Add(lockOutConfig.LockoutDuration); // قفل موقت
                }
                unitOfWork.UserRepository.Update(user);
                await unitOfWork.CommitAsync(cancellationToken);
                return Responses.Fail<LoginResponse>(null, message: "رمز عبور یا نام کاربری اشتباه است.", code: 401);
            }

            // موفقیت در ورود
            user.failed_login_count = 0;
            user.last_login_date_time = DateTime.Now;
            var role = unitOfWork.UserRoleRepository.FindList(x => x.userid == user.id);
            var token = "";
            do
            {
                token = jwtService.GenerateAccessToken(user, role.Select(x => x.role.name).ToList());
            }
            while (await unitOfWork.TokenBlacklistRepository.ExistsAsync(x => x.token == token));
            var refreshToken = "";
            do
            {
                refreshToken = jwtService.GenerateRefreshToken();
            }
            while (await unitOfWork.UserRepository.ExistsAsync(x => x.refresh_token == refreshToken));


            user.refresh_token = refreshToken;
            user.refresh_token_expiry_time = DateTime.Now.Add(Config.AdminRefreshTokenLifetime);

            unitOfWork.UserRepository.Update(user);
            await unitOfWork.CommitAsync(cancellationToken);

            // ذخیره توکن در کوکی
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // موقتا غیر فعال کن برای تست
                SameSite = SameSiteMode.Lax, // یا None اگر لازم بود
                Expires = DateTime.Now.AddMinutes(Config.AdminRefreshTokenLifetime.TotalMinutes)
            };
            var context = httpContextAccessor.HttpContext;
            context.Response.Cookies.Append("jwt", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // موقتا غیر فعال کن برای تست
                SameSite = SameSiteMode.Lax, // یا None اگر لازم بود
                Expires = DateTime.Now.AddMinutes(Config.AdminRefreshTokenLifetime.TotalMinutes)
            });

            return Responses.Success(data:
                new LoginResponse()
                {
                    access_token = token,
                    expire_in = Config.AccessTokenLifetime.TotalMinutes
                },
                message: "ورود با موفقیت انجام شد",
                code: 200
                );
        }
        catch (Exception ex)
        {
            // لاگ‌گیری با Serilog برای ثبت خطاهای غیرمنتظره
            Log.Error(ex, "خطا در زمان ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<LoginResponse>(null, null);
        }
    }
}
