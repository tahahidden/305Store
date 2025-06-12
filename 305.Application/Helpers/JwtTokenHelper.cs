using _305.Application.IService;
using _305.Application.IUOW;
using _305.Domain.Entity;

namespace _305.Application.Helpers;

/// <summary>
/// توابع کمکی برای تولید توکن‌های یکتا
/// </summary>
public static class JwtTokenHelper
{
    /// <summary>
    /// تولید توکن دسترسی که در بلک لیست وجود ندارد
    /// </summary>
    public static async Task<string> GenerateUniqueAccessToken(
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        User user,
        List<string?> roles)
    {
        string token;
        do
        {
            token = jwtService.GenerateAccessToken(user, roles);
        } while (await unitOfWork.TokenBlacklistRepository.ExistsAsync(x => x.token == token));
        return token;
    }

    /// <summary>
    /// تولید رفرش توکن یکتا که در دیتابیس موجود نباشد
    /// </summary>
    public static async Task<string> GenerateUniqueRefreshToken(
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        string refreshToken;
        do
        {
            refreshToken = jwtService.GenerateRefreshToken();
        } while (await unitOfWork.UserRepository.ExistsAsync(x => x.refresh_token == refreshToken));
        return refreshToken;
    }
}
