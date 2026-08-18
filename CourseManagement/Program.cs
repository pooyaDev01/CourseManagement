using AutoMapper;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Application.Mappings;
using CourseManagement.Application.Services;
using CourseManagement.Infrastructure.Data;
using CourseManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Only register SqlServer DbContext if NOT in Test environment
// Test environment will register InMemory in CustomWebApplicationFactory
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<ILessonRepository,LessonRepository>();
builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddScoped<ILessonService,LessonService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"]!;
}, typeof(MappingProfile).Assembly);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { } // make the auto-generated Program accessible programmatically