using System.Reflection;
using Serilog;

namespace _305.Application.Base.Mapper;

/// <summary>
/// کلاس کمکی برای نگاشت (Map) یک شیء منبع به یک شیء مقصد بر اساس پراپرتی‌های هم‌نام و خواندنی/نوشتنی با استفاده از Reflection.
/// </summary>
public static class Mapper
{
	/// <summary>
	/// نگاشت نمونه‌ای از <typeparamref name="TSource"/> به نمونه‌ای از <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">نوع منبع</typeparam>
	/// <typeparam name="TDestination">نوع مقصد</typeparam>
	/// <param name="source">شیء منبع برای نگاشت</param>
	/// <returns>شیء نگاشته شده از نوع مقصد</returns>
	/// <exception cref="ArgumentNullException">اگر شیء منبع null باشد</exception>
	public static TDestination Map<TSource, TDestination>(TSource source)
		where TDestination : class, new()
		where TSource : class
	{
		if (source == null)
			throw new ArgumentNullException(nameof(source), "شیء منبع نمی‌تواند null باشد.");

		var destination = new TDestination();

		var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var sourceProp in sourceProps)
		{
			if (!sourceProp.CanRead)
				continue;

			var destProp = destProps.FirstOrDefault(p =>
				p.Name == sourceProp.Name && p.CanWrite);

			if (destProp == null)
				continue;

			var sourceValue = sourceProp.GetValue(source);

			try
			{
				if (sourceValue == null)
				{
					destProp.SetValue(destination, null);
					continue;
				}

				if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType) ||
					IsNullableMatch(destProp.PropertyType, sourceProp.PropertyType))
				{
					destProp.SetValue(destination, sourceValue);
				}
				else if (IsComplexType(destProp.PropertyType) && IsComplexType(sourceProp.PropertyType))
				{
					var nestedMappedValue = typeof(Mapper)
						.GetMethod(nameof(Map))?
						.MakeGenericMethod(sourceProp.PropertyType, destProp.PropertyType)
						.Invoke(null, new object?[] { sourceValue });

					destProp.SetValue(destination, nestedMappedValue);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "خطا در نگاشت پراپرتی {PropertyName}", sourceProp.Name);
			}
		}

		return destination;
	}

	private static bool IsComplexType(Type type)
		=> type.IsClass && type != typeof(string);

	private static bool IsNullableMatch(Type type1, Type type2)
		=> (IsNullableType(type1) && Nullable.GetUnderlyingType(type1) == type2)
		   || (IsNullableType(type2) && Nullable.GetUnderlyingType(type2) == type1);

	private static bool IsNullableType(Type type)
		=> type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}
