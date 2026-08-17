using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Entities;
using CourseManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course> AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);

        await _context.SaveChangesAsync();

        return course;
    }

    public async Task DeleteAsync(Course course)
    {
        _context.Courses.Remove(course);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);

        await _context.SaveChangesAsync();
    }
}

