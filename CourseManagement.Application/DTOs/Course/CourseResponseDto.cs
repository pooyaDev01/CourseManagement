using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.DTOs.Course;

public class CourseResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }
}

