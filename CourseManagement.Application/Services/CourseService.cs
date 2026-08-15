using AutoMapper;
using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public Task<CourseResponseDto> AddAsync(AddCourseRequestDto courseRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();

        return _mapper.Map<List<CourseResponseDto>>(courses);
    }

    public async Task<CourseResponseDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if(course is null)
        {
            return null;
        }

        return _mapper.Map<CourseResponseDto>(course);
    }

    public Task<CourseResponseDto> UpdateAsync(int id, UpdateCourseRequestDto updateCourseRequestDto)
    {
        throw new NotImplementedException();
    }
}

