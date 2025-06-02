using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.Assistant;
public static class Files
{
    /// <summary>
    /// این متد یک فایل جعلی (Fake) از نوع IFormFile می‌سازد که در تست‌ها کاربرد دارد.
    /// به کمک این فایل می‌توانیم تست‌هایی که نیاز به آپلود فایل دارند را شبیه‌سازی کنیم بدون نیاز به فایل واقعی.
    /// </summary>
    /// <param name="fileName">نام فایل (پیش‌فرض "test.jpg")</param>
    /// <param name="contentType">نوع محتوا یا MIME type فایل (پیش‌فرض "image/jpeg")</param>
    /// <param name="content">محتوای رشته‌ای که داخل فایل قرار می‌گیرد (پیش‌فرض "fake image content")</param>
    /// <returns>یک نمونه از IFormFile که می‌توان در تست‌ها استفاده کرد</returns>
    public static IFormFile CreateFakeFormFile(string fileName = "test.jpg", string contentType = "image/jpeg", string content = "fake image content")
    {
        // تبدیل رشته محتوای فایل به آرایه بایت و سپس ساخت استریم حافظه‌ای
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // ساخت نمونه FormFile با استریم ساخته شده، طول استریم و مشخصات فایل مانند نام و نام فرم فیلد
        return new FormFile(stream, 0, stream.Length, "image_file", fileName)
        {
            // مقداردهی به هدرهای HTTP فایل (خالی به صورت پیش‌فرض)
            Headers = new HeaderDictionary(),

            // تعیین نوع محتوا (Content-Type) برای فایل
            ContentType = contentType
        };
    }
}
