using System.Linq.Expressions;

namespace _305.BuildingBlocks.Helper;
/// <summary>
/// کلاس کمکی برای بررسی و تحلیل عبارات لامبدا (Expressions)
/// </summary>
/// TODO : Use This
public static class ExpressionHelper
{
    /// <summary>
    /// بررسی می‌کند که آیا عبارت شرطی شامل بررسی مقدار فیلد "name" با مقدار مشخص‌شده است یا خیر.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity)</typeparam>
    /// <param name="expression">عبارت لامبدا مانند x => x.name == "Ali"</param>
    /// <param name="expectedName">مقدار مورد انتظار برای name</param>
    /// <returns>در صورت تطابق نام و مقدار، true</returns>
    public static bool CheckExpressionForName<TEntity>(Expression<Func<TEntity, bool>> expression, string expectedName)
    {
        // بررسی اینکه بدنه عبارت یک شرط دودویی باشد (مثل x.name == "Ali")
        if (expression.Body is BinaryExpression { Left: MemberExpression member, Right: ConstantExpression constant })
        {
            // بررسی اینکه فیلد مورد بررسی "name" باشد و مقدار آن برابر expectedName باشد
            return member.Member.Name == "name" && constant.Value?.ToString() == expectedName;
        }

        return false;
    }

    /// <summary>
    /// بررسی می‌کند که آیا عبارت شرطی شامل بررسی مقدار فیلد "slug" با مقدار مشخص‌شده است یا خیر.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity)</typeparam>
    /// <param name="expression">عبارت لامبدا مانند x => x.slug == "product-123"</param>
    /// <param name="expectedSlug">مقدار مورد انتظار برای slug</param>
    /// <returns>در صورت تطابق slug و مقدار، true</returns>
    public static bool CheckExpressionForSlug<TEntity>(Expression<Func<TEntity, bool>> expression, string expectedSlug)
    {
        if (expression.Body is BinaryExpression { Left: MemberExpression member, Right: ConstantExpression constant })
        {
            return member.Member.Name == "slug" && constant.Value?.ToString() == expectedSlug;
        }

        return false;
    }

    /// <summary>
    /// بررسی عمومی برای تطابق مقدار یک ویژگی خاص در عبارت شرطی
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <param name="expression">عبارت شرطی مانند x => x.Property == "Value"</param>
    /// <param name="propertyName">نام ویژگی مورد بررسی</param>
    /// <param name="expectedValue">مقدار مورد انتظار برای ویژگی</param>
    /// <returns>در صورت تطابق نام ویژگی و مقدار آن، true</returns>
    public static bool TestExpressionMatch<TEntity>(
        Expression<Func<TEntity, bool>> expression,
        string propertyName,
        string expectedValue
    )
    {
        if (expression.Body is BinaryExpression { Left: MemberExpression memberExpr, Right: ConstantExpression constantExpr })
        {
            return memberExpr.Member.Name == propertyName &&
                   constantExpr.Value?.ToString() == expectedValue;
        }

        return false;
    }
}
