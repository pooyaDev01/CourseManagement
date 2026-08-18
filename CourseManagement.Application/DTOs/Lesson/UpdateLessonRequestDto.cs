using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseManagement.Application.DTOs.Lesson;
public class UpdateLessonRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600)]
    public int DurationInMinutes { get; set; }
    [Required]
    public int CourseId { get; set; }
}

