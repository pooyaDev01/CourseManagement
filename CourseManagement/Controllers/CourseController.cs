using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseResponseDto>> GetById([FromRoute]int id)
    {
        var course =  await _courseService.GetByIdAsync(id);

        if (course is null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpGet]
    public async Task<ActionResult<CourseResponseDto>> GetAll()
    {
        var courses = await _courseService.GetAllAsync();

        return Ok(courses);
    }

    [HttpPost]
    public async Task<ActionResult<CourseResponseDto>> Add([FromBody] AddCourseRequestDto dto)
    {
        var createdCourse = await _courseService.AddAsync(dto);

         return CreatedAtAction(nameof(GetById), new {id = createdCourse.Id}, createdCourse );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CourseResponseDto>> Update([FromRoute] int id ,[FromBody] UpdateCourseRequestDto dto)
    {
        var updatedCourse = await _courseService.UpdateAsync(id, dto);

        if (updatedCourse is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult<CourseResponseDto>> Delete([FromRoute] int id)
    {
        var deletedCourse = await _courseService.DeleteAsync(id);

        if (deletedCourse == false)
        {
            return NotFound();
        }

        return NoContent();
    }


}

