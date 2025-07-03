# Health Check Setup

برای استفاده از HealthCheck های جدید مراحل زیر را دنبال کنید:

1. در پروژه `305.WebApi` پکیج های زیر را نصب کنید:
   - `AspNetCore.HealthChecks.SqlServer` برای بررسی دیتابیس SQL Server
   - `AspNetCore.HealthChecks.System` برای چک های CPU و حافظه
2. پس از نصب پکیج ها دستور `dotnet restore` را اجرا کنید.
3. در صورت نیاز مقادیر آستانه های حافظه، CPU و فضای دیسک را در کلاس های موجود در مسیر `305.WebApi/HealthChecks` تغییر دهید.
4. برای مشاهده وضعیت سیستم، endpoint جدید `/health` را فراخوانی کنید.
