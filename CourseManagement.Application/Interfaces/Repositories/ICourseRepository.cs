using CourseManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Interfaces.Repositories;
public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync(); 

    Task<Course?> GetByIdAsync(int id);

    Task<Course> AddAsync(Course course);

    Task UpdateAsync(Course course);

    Task DeleteAsync(Course course);

}

