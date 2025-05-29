# ✅ TODO List – پروژه 305

فهرستی از کارهای فنی و معماری برای بهبود پروژه

---

## 🧱 معماری و ساختار پروژه

- [ ] ایجاد پروژه جدید `MyProject.Contracts` برای تعریف:
  - [ ] DTOهای مشترک
  - [ ] Interfaceهای سرویس‌ها (مثلاً IEmailService، IJwtTokenService)
- [ ] جداسازی کامل WebApi از Application با استفاده از قراردادها
- [ ] استفاده از `ValueObjects` در لایه Domain برای مقادیر مفهومی مانند:
  - [ ] Email
  - [ ] PhoneNumber
  - [ ] Money

---

## 🔌 تست‌پذیری و Maintainability

- [ ] تعریف `IDateTimeProvider`, `ICurrentUserService` و پیاده‌سازی آن‌ها در Infrastructure
- [ ] تزریق اینترفیس‌ها به CommandHandlerها برای قابلیت Mock
- [ ] افزایش پوشش تست (Code Coverage) در پروژه `Tests.Unit`
- [ ] استفاده از ابزار Coverlet + ReportGenerator برای بررسی پوشش تست
- [ ] نوشتن تست‌های یونیت برای:
  - [ ] CommandHandlerها
  - [ ] Validationها
  - [ ] سرویس‌های داخلی مانند JwtTokenService

---

## 🎯 قابلیت‌های اپلیکیشن

- [ ] استفاده از FluentValidation برای Command/Queryهای Application
- [ ] پیاده‌سازی Domain Events برای اتفاقات مهم در دامنه (مثلاً UserRegistered)
- [ ] استفاده از MediatR Behaviors برای:
  - [ ] Validation
  - [ ] Logging
  - [ ] Performance Profiling

---

## ⚙️ عملکرد و کشینگ

- [ ] اضافه‌کردن `ICacheService` و پیاده‌سازی Redis یا InMemoryCache
- [ ] استفاده از `AsNoTracking()` در Queryهای فقط خواندنی
- [ ] بهینه‌سازی EF Core Split Queries در صورت نیاز

---

## 🧪 تست‌های یکپارچه

- [ ] تست‌های Integration برای APIهای مهم در پروژه `Tests.Integration`
- [ ] تست کردن خروجی JSON، وضعیت‌ها (200, 400, 401, 500) و دیتا

---

## 🌐 Web API بهبود

- [ ] اضافه کردن نسخه‌بندی API (مثلاً `/api/v1`)
- [ ] فعال‌سازی Swagger با Security و Token Input
- [ ] اضافه کردن Exception Handling Middleware با خروجی ساخت‌یافته
- [ ] استفاده از Serilog برای Structured Logging

---

## 🚀 CI/CD و DevOps

- [ ] ایجاد فایل `.github/workflows/ci.yml` برای اجرای:
  - [ ] Build
  - [ ] Test
  - [ ] Code Style
- [ ] تولید اسکریپت‌های Migration جداگانه در پروژه `MyProject.Database`

---

## 🔮 قابلیت‌های پیشرفته (اختیاری)

- [ ] پیاده‌سازی Outbox Pattern برای ارسال Eventها
- [ ] طراحی Feature Folders در WebApi (جایگزین لایه‌بندی عمودی)
- [ ] جداکردن Configها بر اساس محیط (`appsettings.Development.json`, `Staging`, ...)

---

## 📝 مستندات و راهنما

- [ ] تکمیل README.md با:
  - [ ] توضیح معماری
  - [ ] نحوه اجرای پروژه
  - [ ] ساختار فولدرها و وابستگی‌ها
- [ ] مستندسازی APIها با Swagger و NSwag

---

> آخرین به‌روزرسانی: ۲ خرداد ۱۴۰۴
