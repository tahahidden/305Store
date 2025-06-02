using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Application.Base.Validator;
/// <summary>
/// کلاس نمایانگر یک آیتم اعتبارسنجی که شامل یک قانون اعتبارسنجی (تابعی که مقدار بولی async برمی‌گرداند)
/// و پیغام یا مقدار مرتبط با آن است.
/// </summary>
public class ValidationItem
{
    /// <summary>
    /// قانون اعتبارسنجی به صورت یک تابع async که نتیجه آن true به معنی عدم اعتبار یا تکراری بودن است.
    /// </summary>
    public Func<Task<bool>> Rule { get; set; }

    /// <summary>
    /// مقدار یا پیغام مرتبط با این قانون اعتبارسنجی که معمولا برای نمایش به کاربر استفاده می‌شود.
    /// </summary>
    public string Value { get; set; }
}
