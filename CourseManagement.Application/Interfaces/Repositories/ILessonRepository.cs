using CourseManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Interfaces.Repositories;
public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetAllAsync();

    Task<Lesson?> GetByIdAsync(int id);

    Task<Lesson> AddAsync(Lesson lesson);

    Task UpdateAsync(Lesson lesson);

    Task DeleteAsync(Lesson lesson);
}

