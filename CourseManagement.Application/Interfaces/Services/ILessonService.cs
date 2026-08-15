using CourseManagement.Application.DTOs.Lesson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Interfaces.Services;

public interface ILessonService
{
    Task<IEnumerable<LessonResponseDto>> GetAllAsync();
    Task<LessonResponseDto?> GetByIdAsync(int id);
    Task<LessonResponseDto> AddAsync(AddLessonRequestDto lessonRequestDto);
    Task<LessonResponseDto> UpdateAsync(int id, UpdateLessonRequestDto updateLessonRequestDto);
    Task<bool> DeleteAsync(int id);
}

