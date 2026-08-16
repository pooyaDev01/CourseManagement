using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Application.Mappings;
using CourseManagement.Application.Services;
using CourseManagement.Entities;
using CourseManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
