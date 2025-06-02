using System.Security.Claims;
using System.Text.Json;

namespace _305.BuildingBlocks.Helper
{
    public static class JwtParser
    {
        // متد اصلی برای استخراج ادعاها (Claims) از توکن JWT
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                throw new ArgumentException("JWT token is null or empty.");

            // تقسیم توکن به سه قسمت: header, payload و signature
            var parts = jwt.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token must consist of header, payload, and signature.");

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // تبدیل بایت‌های payload به دیکشنری کلید-مقدار
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes, options)
                                ?? new Dictionary<string, object>();

            var claims = new List<Claim>();

            // استخراج نقش‌ها به صورت خاص از JWT
            ExtractRolesFromJwt(claims, keyValuePairs);

            // تبدیل سایر کلید-مقدارها به ادعاهای معمولی
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value == null) continue;

                if (kvp.Value is JsonElement element)
                {
                    string valueString = element.ValueKind switch
                    {
                        JsonValueKind.String => element.GetString() ?? string.Empty,
                        JsonValueKind.Number => element.GetRawText(),
                        JsonValueKind.True => "true",
                        JsonValueKind.False => "false",
                        _ => element.GetRawText()
                    };
                    claims.Add(new Claim(kvp.Key, valueString));
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
                }
            }

            return claims;
        }

        // متد کمکی برای پردازش رشته Base64 که ممکن است padding نداشته باشد
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        // استخراج نقش‌ها (Roles) از دیکشنری کلید-مقدار به صورت ویژه
        private static void ExtractRolesFromJwt(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            if (!keyValuePairs.TryGetValue(ClaimTypes.Role, out var rolesObj))
                return;

            if (rolesObj == null)
                return;

            // نقش‌ها ممکن است به صورت رشته یا آرایه باشند
            if (rolesObj is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var roleElement in jsonElement.EnumerateArray())
                    {
                        var role = roleElement.GetString();
                        if (!string.IsNullOrEmpty(role))
                            claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                else if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    var rolesStr = jsonElement.GetString() ?? string.Empty;
                    AddRolesFromString(claims, rolesStr);
                }
            }
            else if (rolesObj is string rolesStr)
            {
                AddRolesFromString(claims, rolesStr);
            }
            else
            {
                // حالت fallback: تبدیل به رشته و پردازش
                AddRolesFromString(claims, rolesObj.ToString() ?? string.Empty);
            }

            // حذف نقش‌ها از دیکشنری تا دوباره به ادعاها اضافه نشوند
            keyValuePairs.Remove(ClaimTypes.Role);
        }

        // متد کمکی برای افزودن نقش‌ها از رشته نقش‌ها (مثلاً ["admin","user"])
        private static void AddRolesFromString(List<Claim> claims, string rolesStr)
        {
            var roles = rolesStr.Trim().TrimStart('[').TrimEnd(']').Split(',');
            foreach (var role in roles)
            {
                var trimmedRole = role.Trim().Trim('"');
                if (!string.IsNullOrEmpty(trimmedRole))
                    claims.Add(new Claim(ClaimTypes.Role, trimmedRole));
            }
        }
    }
}
