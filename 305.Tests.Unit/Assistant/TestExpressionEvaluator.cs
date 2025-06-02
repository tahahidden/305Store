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
		// بررسی اینکه بدنه Expression یک BinaryExpression (عملگر دودویی مثل ==) باشد
		if (expr.Body is not BinaryExpression binary ||
			// سمت چپ Expression باید یک MemberExpression (فیلد یا پراپرتی) باشد
			binary.Left is not MemberExpression member ||
			// نام فیلد سمت چپ باید "slug" باشد (بدون توجه به حروف بزرگ و کوچک)
			!member.Member.Name.Equals("slug", StringComparison.OrdinalIgnoreCase)) return false;
		// گرفتن مقدار سمت راست Expression (مقدار مورد مقایسه)
		var rightValue = GetValueFromExpression(binary.Right);

		// مقایسه مقدار سمت راست با مقدار مورد انتظار
		return rightValue?.ToString() == expectedSlug;

		// اگر شرایط بالا برقرار نبود، false برمی‌گرداند
	}

	/// <summary>
	/// این متد مقدار واقعی (value) یک Expression را استخراج می‌کند،
	/// مخصوصا اگر Expression از نوع ConstantExpression یا MemberExpression باشد.
	/// </summary>
	/// <param name="expression">عبارت Expression که باید مقدارش استخراج شود</param>
	/// <returns>مقدار استخراج شده از Expression یا null اگر مقدار قابل استخراج نباشد</returns>
	private static object? GetValueFromExpression(Expression expression)
	{
		switch (expression)
		{
			// اگر Expression از نوع ثابت باشد، مقدار آن را مستقیما برمی‌گرداند
			case ConstantExpression constant:
				return constant.Value;
			// اگر Expression یک MemberExpression باشد (مثلا فیلدی که مقدارش در حافظه است)
			case MemberExpression memberExpression:
				{
					// تبدیل MemberExpression به یک Lambda Expression که مقدار را به صورت آبجکت برمی‌گرداند
					var objectMember = Expression.Convert(memberExpression, typeof(object));
					var getterLambda = Expression.Lambda<Func<object>>(objectMember);
					var getter = getterLambda.Compile();

					// اجرای Lambda برای به دست آوردن مقدار
					return getter();
				}
			default:
				// اگر نتوان مقدار را استخراج کرد، null برمی‌گرداند
				return null;
		}
	}
}
