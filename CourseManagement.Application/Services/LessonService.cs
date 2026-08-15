using AutoMapper;
using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Services;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    public LessonService(ILessonRepository lessonRepository, IMapper mapper)
    {
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<LessonResponseDto> AddAsync(AddLessonRequestDto lessonRequestDto)
    {
        var lesson = _mapper.Map<Lesson>(lessonRequestDto);

        var createdLesson = await _lessonRepository.AddAsync(lesson);

        return _mapper.Map<LessonResponseDto>(createdLesson);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson is null)
        {
            return false;
        }

        await _lessonRepository.DeleteAsync(lesson);

        return true;
    }

    public async Task<IEnumerable<LessonResponseDto>> GetAllAsync()
    {
        var lessons = await _lessonRepository.GetAllAsync();

        var response = _mapper.Map<List<LessonResponseDto>>(lessons);

        return response;
    }

    public async Task<LessonResponseDto?> GetByIdAsync(int id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson is null)
        {
            return null;
        }

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task<LessonResponseDto?> UpdateAsync(int id, UpdateLessonRequestDto updateLessonRequestDto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson is null)
        {
            return null;
        }

        _mapper.Map(updateLessonRequestDto, lesson);

        await _lessonRepository.UpdateAsync(lesson);

        return _mapper.Map<LessonResponseDto>(lesson);
    }
}

