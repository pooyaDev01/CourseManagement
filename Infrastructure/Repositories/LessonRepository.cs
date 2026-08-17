using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Entities;
using CourseManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson> AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);

        await _context.SaveChangesAsync();

        return lesson;
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        _context.Lessons.Remove(lesson);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _context.Lessons.ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);

        await _context.SaveChangesAsync();
    }
}

