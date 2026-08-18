using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagement.Integrations.Tests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descripter = services.SingleOrDefault(temp => temp.ServiceType ==
            typeof(DbContextOptions<AppDbContext>));

            if (descripter != null)
            {
                services.Remove(descripter);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("DatabaseForTesting"); //it means this ef-core in memory, everytime executes for testing, it will regenerate the new and empty databse for integration testing
            });
        });
    }
}

