using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.DTOs.Lesson;
public class LessonResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public int CourseId { get; set; }
}

