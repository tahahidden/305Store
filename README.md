note: تمام کامنت های این پروژه جنبه ی آموزشی دارند

ساختار کلی پروژه:

MyProject.sln │ ├── MyProject.Domain 👈 منطق دامنه (Core DDD Concepts) │ ├── Entities │ ├── ValueObjects │ ├── Aggregates │ ├── Interfaces (Contracts like IRepository, IDomainEventPublisher) │ └── Events

├── MyProject.Application 👈 لایه‌ی اپلیکیشن (بدون منطق بیزینسی) │ ├── Commands (CQRS → Create, Update, Delete) │ ├── Queries (CQRS → Read) │ ├── DTOs │ ├── Interfaces (IService, IUseCase) │ ├── Mappings (AutoMapper config) │ └── Validators (FluentValidation)

├── MyProject.Infrastructure 👈 زیرساخت (EF Core, SMTP, SMS, Logging, ...) │ ├── Persistence (EF Core DbContext, Repositories) │ ├── Services (EmailService, FileUploader, ...) │ ├── Configurations (EF EntityTypeConfigurations) │ └── ExternalIntegrations

├── MyProject.WebApi 👈 واسط (API, Controller) │ ├── Controllers │ ├── Middlewares │ ├── Extensions (DI, Swagger, CORS, etc.) │ └── Program.cs & Startup.cs

├── MyProject.Tests.Unit 👈 تست‌های واحد (Domain, Application) ├── MyProject.Tests.Integration 👈 تست‌های یکپارچه (Web + Infrastructure) └── MyProject.BuildingBlocks 👈 کدهای مشترک و پایه (Cross-Cutting Concerns) ├── BaseEntity, Result ├── GuardClauses ├── ValueObject base class └── Interfaces (ILoggerAdapter, IClock, etc.)

