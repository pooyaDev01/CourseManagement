using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CourseManagement.Integrations.Tests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set environment FIRST, before base.ConfigureWebHost
        builder.UseEnvironment("Test");

        // Now call base which will run Program.cs with "Test" environment
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Remove any leftover SqlServer DbContextOptions
            var dbContextOptionsDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextOptionsDescriptor != null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            // Also try to remove DbContextOptions (base class) if it was registered
            var baseDbContextOptionsDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(DbContextOptions));

            if (baseDbContextOptionsDescriptor != null)
            {
                services.Remove(baseDbContextOptionsDescriptor);
            }

            // Now add the InMemory database configuration
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb_");
            });
        });
    }
}

