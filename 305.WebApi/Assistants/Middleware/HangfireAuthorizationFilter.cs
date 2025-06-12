using Hangfire.Dashboard;

namespace _305.WebApi.Assistants.Middleware
{
	/// <summary>
	/// فیلتر مجوز برای دسترسی به داشبورد Hangfire، که تنها کاربران احراز هویت‌شده
	/// و دارای نقش‌های مجاز را می‌پذیرد.
	/// </summary>
	public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		private readonly string[] _allowedRoles;

		/// <summary>
		/// سازنده فیلتر مجوز Hangfire.
		/// </summary>
		/// <param name="allowedRoles">لیست نقش‌های مجاز. اگر خالی باشد، تنها احراز هویت کافی است.</param>
		public HangfireAuthorizationFilter(params string[] allowedRoles)
		{
			_allowedRoles = allowedRoles ?? [];
		}

		/// <summary>
		/// بررسی مجوز دسترسی کاربر به داشبورد Hangfire.
		/// </summary>
		/// <param name="context">Context داشبورد Hangfire</param>
		/// <returns>در صورت داشتن مجوز، مقدار true بازمی‌گرداند.</returns>
		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();
			var user = httpContext.User;

			// اگر کاربر احراز هویت نشده باشد، رد شود
			if (user?.Identity?.IsAuthenticated != true)
				return false;

			// اگر لیست نقش‌ها خالی باشد، فقط احراز هویت کافی است
			if (_allowedRoles.Length == 0)
				return true;

			// بررسی اینکه آیا کاربر حداقل یکی از نقش‌های مجاز را دارد
			return _allowedRoles.Any(role => user.IsInRole(role));
		}
	}
}