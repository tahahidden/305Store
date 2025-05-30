using _305.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Integration.Base;
public abstract class ControllerBaseTests
{
    protected readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private const string connectionString = "Server=.;Database=305_test_db;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";

    public ControllerBaseTests()
    {
        var _webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                services.Remove(descriptor);
                services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
            });
        });

        _serviceProvider = _webApplicationFactory.Services;
        _httpClient = _webApplicationFactory.CreateClient();
    }

    public ApplicationDbContext GetContext()
    {
        return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    protected void StartDatabase()
    {
        var sampleLibraryContext = GetContext();
        sampleLibraryContext.Database.Migrate();
        
    }

    protected virtual void SeedData(params object[] data)
    {
        var sampleLibraryContext = GetContext();
        sampleLibraryContext.AddRange(data);
        sampleLibraryContext.SaveChanges();
    }

    protected void ResetDatabase()
    {
        var sampleLibraryContext = GetContext();
        sampleLibraryContext.Database.ExecuteSqlRaw("delete BlacklistedToken");
        sampleLibraryContext.Database.ExecuteSqlRaw("delete Blog");
        sampleLibraryContext.Database.ExecuteSqlRaw("delete BlogCategory");
		sampleLibraryContext.Database.ExecuteSqlRaw("delete Permission");
		sampleLibraryContext.Database.ExecuteSqlRaw("delete Role");
		sampleLibraryContext.Database.ExecuteSqlRaw("delete RolePermission");
		sampleLibraryContext.Database.ExecuteSqlRaw("delete User");
		sampleLibraryContext.Database.ExecuteSqlRaw("delete UserRole");
	}
}