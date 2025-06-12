namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// مشخص‌کننده مجوز لازم برای اجرای اکشن.
/// این Attribute برای تزئین متدهای کنترلر استفاده می‌شود.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class PermissionAttribute(string name) : Attribute
{
	/// <summary>
	/// نام مجوز مورد نیاز برای اجرای این متد.
	/// </summary>
	public string Name { get; } = name;
}