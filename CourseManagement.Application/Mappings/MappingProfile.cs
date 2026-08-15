using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Entities;

namespace CourseManagement.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCourseRequestDto, Course>();

        CreateMap<UpdateCourseRequestDto, Course>();

        CreateMap<Course, CourseResponseDto>();

        CreateMap<AddLessonRequestDto, Lesson>();

        CreateMap<UpdateLessonRequestDto, Lesson>();

        CreateMap<Lesson, LessonResponseDto>();
    }
}
