using System.Linq.Expressions;

namespace _305.Tests.Unit.Assistant;
public static class TestExpressionEvaluator
{
    /// <summary>
    /// این متد دو عبارت (Expression) بولی را به صورت رشته مقایسه می‌کند
    /// و بررسی می‌کند که آیا دو Expression ورودی از نظر ساختاری یکسان هستند یا خیر.
    /// توجه: این مقایسه فقط بر اساس رشته نمایشی Expression است و مقایسه عمیق ساختاری نیست.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity) که Expression روی آن تعریف شده</typeparam>
    /// <param name="actualExpr">عبارت فعلی که باید بررسی شود</param>
    /// <param name="expectedExpr">عبارت مورد انتظار برای مقایسه</param>
    /// <returns>اگر رشته نمایشی دو Expression برابر بود، true برمی‌گرداند، در غیر اینصورت false</returns>
    public static bool MatchPredicate<TEntity>(Expression<Func<TEntity, bool>> actualExpr, Expression<Func<TEntity, bool>> expectedExpr)
    {
        return actualExpr.ToString() == expectedExpr.ToString();
    }

    /// <summary>
    /// این متد بررسی می‌کند که آیا Expression ورودی یک شرط بر روی فیلد "slug" است
    /// و مقدار سمت راست آن با مقدار expectedSlug برابر است یا خیر.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت که Expression روی آن تعریف شده</typeparam>
    /// <param name="expr">عبارت شرطی که باید بررسی شود</param>
    /// <param name="expectedSlug">مقدار مورد انتظار برای فیلد "slug"</param>
    /// <returns>اگر شرط روی فیلد slug باشد و مقدار آن برابر expectedSlug باشد true برمی‌گرداند، در غیر اینصورت false</returns>
    public static bool MatchSlugExpression<TEntity>(Expression<Func<TEntity, bool>> expr, string expectedSlug)
    {
        // بررسی ساختار شرط و استخراج مقدار سمت راست در صورت معتبر بودن
        if (expr.Body is BinaryExpression { Left: MemberExpression { Member.Name: var name }, Right: var right }
            && name.Equals("slug", StringComparison.OrdinalIgnoreCase))
        {
            var rightValue = GetValueFromExpression(right);
            return rightValue?.ToString() == expectedSlug;
        }

        return false;

        // اگر شرایط بالا برقرار نبود، false برمی‌گرداند
    }

    /// <summary>
    /// این متد مقدار واقعی (value) یک Expression را استخراج می‌کند،
    /// مخصوصا اگر Expression از نوع ConstantExpression یا MemberExpression باشد.
    /// </summary>
    /// <param name="expression">عبارت Expression که باید مقدارش استخراج شود</param>
    /// <returns>مقدار استخراج شده از Expression یا null اگر مقدار قابل استخراج نباشد</returns>
    private static object? GetValueFromExpression(Expression expression) => expression switch
    {
        ConstantExpression constant => constant.Value,
        MemberExpression memberExpression =>
            Expression.Lambda<Func<object>>(Expression.Convert(memberExpression, typeof(object))).Compile().Invoke(),
        _ => null,
    };
}
