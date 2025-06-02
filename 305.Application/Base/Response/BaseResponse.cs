namespace _305.Application.Base.Response;

/// <summary>
/// کلاس پایه برای پاسخ‌ها که شامل اطلاعات مشترک مانند شناسه، نام، نامک و تاریخ‌های ایجاد و بروزرسانی است.
/// </summary>
public class BaseResponse
{
	/// <summary>
	/// شناسه یکتا
	/// </summary>
	public long id { get; set; }

	/// <summary>
	/// نام مورد نظر
	/// </summary>
	public string name { get; set; }

	/// <summary>
	/// نامک (Slug) برای شناسه قابل خواندن در URL
	/// </summary>
	public string slug { get; set; }

	/// <summary>
	/// زمان ایجاد رکورد
	/// </summary>
	public DateTime created_at { get; set; }

	/// <summary>
	/// زمان آخرین بروزرسانی رکورد
	/// </summary>
	public DateTime updated_at { get; set; }
}