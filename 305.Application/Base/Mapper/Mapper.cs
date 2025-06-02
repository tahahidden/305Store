using System.Reflection;

namespace _305.Application.Base.Mapper;

/// <summary>
/// کلاس کمکی برای انجام عملیات نگاشت (Map) از یک شیء منبع به یک شیء مقصد.
/// این نگاشت با بازتاب (Reflection) انجام می‌شود و پراپرتی‌های هم‌نام و قابل خواندن/نوشتن را انتقال می‌دهد.
/// </summary>
public static class Mapper
{
	/// <summary>
	/// نگاشت یک نمونه از نوع <typeparamref name="TSource"/> به نمونه‌ای از نوع <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">نوع منبع</typeparam>
	/// <typeparam name="TDestination">نوع مقصد</typeparam>
	/// <param name="source">شیء منبع برای نگاشت</param>
	/// <returns>شیء نگاشته شده از نوع مقصد</returns>
	/// <exception cref="ArgumentNullException">اگر شیء منبع برابر null باشد پرتاب می‌شود</exception>
	public static TDestination Map<TSource, TDestination>(TSource source)
		where TDestination : class, new()
		where TSource : class
	{
		if (source == null)
			throw new ArgumentNullException(nameof(source), "Source object cannot be null.");

		var destination = new TDestination();

		// گرفتن پراپرتی‌های عمومی و نمونه‌ای از نوع منبع و مقصد
		var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var sourceProp in sourceProps)
		{
			if (!sourceProp.CanRead) continue;

			// پیدا کردن پراپرتی مقصد که نامش با پراپرتی منبع برابر است و قابلیت نوشتن دارد
			var destProp = destProps.FirstOrDefault(p =>
				p.Name == sourceProp.Name &&
				p.CanWrite);

			if (destProp != null)
			{
				var sourceValue = sourceProp.GetValue(source);

				if (sourceValue == null)
				{
					// مقداردهی null اگر مقدار منبع null باشد
					destProp.SetValue(destination, null);
					continue;
				}

				try
				{
					// اگر نوع پراپرتی مقصد قابل انتساب از نوع پراپرتی منبع باشد، مقداردهی مستقیم انجام شود
					if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
					{
						destProp.SetValue(destination, sourceValue);
					}
					// اگر یک طرف Nullable باشد و طرف دیگر نوع اصلی آن، مقداردهی مستقیم انجام شود
					else if (IsNullableMatch(destProp.PropertyType, sourceProp.PropertyType))
					{
						destProp.SetValue(destination, sourceValue);
					}
					// اگر هر دو نوع کلاس پیچیده هستند (یعنی تو در تو)، نگاشت بازگشتی (Recursive) انجام شود
					else if (IsComplexType(destProp.PropertyType) && IsComplexType(sourceProp.PropertyType))
					{
						var nestedMappedValue = typeof(Mapper)
							.GetMethod("Map")
							?.MakeGenericMethod(sourceProp.PropertyType, destProp.PropertyType)
							.Invoke(null, [sourceValue]);

						destProp.SetValue(destination, nestedMappedValue);
					}
				}
				catch
				{
					// TODO: در صورت نیاز، خطای نگاشت را لاگ کنید و ادامه دهید
					continue;
				}
			}
		}

		return destination;
	}

	/// <summary>
	/// بررسی می‌کند که آیا نوع داده یک کلاس پیچیده (غیر از رشته) است یا خیر.
	/// </summary>
	/// <param name="type">نوع داده برای بررسی</param>
	/// <returns>در صورتی که نوع کلاس باشد و رشته نباشد، true برمی‌گرداند</returns>
	private static bool IsComplexType(Type type)
	{
		return type.IsClass && type != typeof(string);
	}

	/// <summary>
	/// بررسی می‌کند که آیا دو نوع یکی nullable و دیگری نوع اصلی آن است.
	/// </summary>
	/// <param name="type1">نوع اول</param>
	/// <param name="type2">نوع دوم</param>
	/// <returns>اگر یکی nullable و دیگری نوع اصلی آن باشد true برمی‌گرداند</returns>
	private static bool IsNullableMatch(Type type1, Type type2)
	{
		// بررسی می‌کند اگر type1 nullable است نوع اصلی آن برابر type2 باشد
		if (IsNullableType(type1) && Nullable.GetUnderlyingType(type1) == type2)
			return true;

		// بررسی می‌کند اگر type2 nullable است نوع اصلی آن برابر type1 باشد
		return IsNullableType(type2) && Nullable.GetUnderlyingType(type2) == type1;
	}

	/// <summary>
	/// بررسی می‌کند که آیا نوع داده Nullable است یا خیر.
	/// </summary>
	/// <param name="type">نوع داده برای بررسی</param>
	/// <returns>اگر نوع Nullable باشد true برمی‌گرداند</returns>
	private static bool IsNullableType(Type type)
	{
		return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
	}
}
