using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<ActionResult<LessonResponseDto>> GetAll()
    {
        var lessons = await _lessonService.GetAllAsync();

        return Ok(lessons);
    }

    [HttpGet("{id :int}")]
    public async Task<ActionResult<LessonResponseDto>> GetById(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);

        if (lesson is null)
        {
            return NotFound();
        }

        return Ok(lesson);
    }

    [HttpPost]
    public async Task<ActionResult<LessonResponseDto>> Add([FromBody] AddLessonRequestDto addLessonRequestDto)
    {
        var createdLesson = await _lessonService.AddAsync(addLessonRequestDto);

        return CreatedAtAction(nameof(GetById), new { id = createdLesson.Id }, createdLesson);
    }

    [HttpPut("{id :int}")]
    public async Task<ActionResult<LessonResponseDto>> Update([FromRoute] int id, [FromBody] UpdateLessonRequestDto updateLessonRequestDto)
    {
        var updatedLesson = await _lessonService.UpdateAsync(id, updateLessonRequestDto);

        if (updatedLesson is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id :int}")]
    public async Task<ActionResult<LessonResponseDto>> Delete([FromRoute] int id)
    {
        var deletedCourse = await _lessonService.DeleteAsync(id);

        if (deletedCourse == false)
        {
            return NotFound();
        }

        return NoContent();
    }

}

