using CourseManagement.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Interfaces.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllAsync();

    Task<CourseResponseDto?> GetByIdAsync(int id);

    Task<CourseResponseDto> AddAsync(AddCourseRequestDto courseRequestDto);

    Task<CourseResponseDto> UpdateAsync(int id, UpdateCourseRequestDto updateCourseRequestDto);

    Task<bool> DeleteAsync(int id);
}
