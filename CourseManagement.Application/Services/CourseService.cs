using AutoMapper;
using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Entities;
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

    public async Task<CourseResponseDto> AddAsync(AddCourseRequestDto courseRequestDto)
    {
        var person = _mapper.Map<Course>(courseRequestDto);

        var course = await _courseRepository.AddAsync(person);

        var response = _mapper.Map<CourseResponseDto>(course);

        return response;
    }

    public Task<bool?> DeleteAsync(int id)
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

    public async Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseRequestDto updateCourseRequestDto)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course is null)
        {
            return null;
        }

        _mapper.Map(updateCourseRequestDto, course); // (Source, Destination) this mapping overload wont create the new refrence

        await _courseRepository.UpdateAsync(course);

        return _mapper.Map<CourseResponseDto>(course); // <Destination>(Source) this mapping overload will create the new refrence
    }
}

