using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace _305.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lightdb;Username=postgres;Password=Abcd1234");

            // برای PostgreSQL
            // optionsBuilder.UseNpgsql("Host=localhost;Database=305Db;Username=postgres;Password=YourPassword");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}