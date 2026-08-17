using AutoFixture;
using CourseManagement.Api.Controllers;
using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Api.Tests;

public class LessonControllerTest
{
    private readonly Mock<ILessonService> _lessonServiceMock;

    private readonly LessonController _lessonController;

    private readonly Fixture _fixture;

    public LessonControllerTest()
    {
        _lessonServiceMock = new Mock<ILessonService>();

        _lessonController = new LessonController(_lessonServiceMock.Object);

        _fixture = new Fixture();
    }
    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenLessonsExists()
    {
        // Arrange
        var lessons = _fixture.CreateMany<LessonResponseDto>(5).ToList();

        _lessonServiceMock.Setup(srv => srv.GetAllAsync())
            .ReturnsAsync(lessons);

        // Act
        var result = await _lessonController.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(lessons);
        _lessonServiceMock.Verify(srv => srv.GetAllAsync(), Times.Once());
    }

    [Fact]
    public async Task GetById_Should_ReturnOk_WhenCourseExists()
    {
        // Arrange
        var dto = _fixture.Create<LessonResponseDto>();

        _lessonServiceMock.Setup(srv => srv.GetByIdAsync(dto.Id))
            .ReturnsAsync(dto);

        // Act
        var result = await _lessonController.GetById(dto.Id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(dto);
        _lessonServiceMock.Verify(srv => srv.GetByIdAsync(dto.Id), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        _lessonServiceMock.Setup(srv => srv.GetByIdAsync(lessonId))
            .ReturnsAsync((LessonResponseDto?)null);

        // Act
        var result = await _lessonController.GetById(lessonId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _lessonServiceMock.Verify(srv => srv.GetByIdAsync(lessonId), Times.Once);
    }

    [Fact]
    public async Task Add_Should_CreateAndReturnSuccessfully()
    {
        // Arrange
        var dto = _fixture.Create<AddLessonRequestDto>();

        var expected = new LessonResponseDto
        {
            Title = dto.Title,
            DurationInMinutes = dto.DurationInMinutes,
            CourseId = dto.CourseId
        };

        _lessonServiceMock.Setup(srv => srv.AddAsync(dto))
            .ReturnsAsync(expected);

        // Act
        var result = await _lessonController.Add(dto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>().Which.Value.Should().BeEquivalentTo(expected);
        _lessonServiceMock.Verify(srv => srv.AddAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Update_Should_UpdateSuccessfully_WhenCourseExists()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateLessonRequestDto>();

        var expected = new LessonResponseDto
        {
            Title = dto.Title,
            DurationInMinutes = dto.DurationInMinutes,
            CourseId = dto.CourseId
        };

        _lessonServiceMock.Setup(srv => srv.UpdateAsync(lessonId, dto))
            .ReturnsAsync(expected);

        // Act
        var result = await _lessonController.Update(lessonId, dto);

        // Assert
        result.Result.Should().BeOfType<NoContentResult>();
        _lessonServiceMock.Verify(srv => srv.UpdateAsync(lessonId, dto), Times.Once);
    }

    [Fact]
    public async Task Update_Should_ReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateLessonRequestDto>();

        _lessonServiceMock.Setup(srv => srv.UpdateAsync(lessonId, dto))
            .ReturnsAsync((LessonResponseDto?)null);

        // Act
        var result = await _lessonController.Update(lessonId, dto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _lessonServiceMock.Verify(srv => srv.UpdateAsync(lessonId, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenCourseExists()
    {
        // Arrange
        var lesonId = _fixture.Create<int>();

        _lessonServiceMock.Setup(srv => srv.DeleteAsync(lesonId))
            .ReturnsAsync(true);

        // Act
        var result = await _lessonController.Delete(lesonId);

        // Assert
        result.Result.Should().BeOfType<NoContentResult>();
        _lessonServiceMock.Verify(srv => srv.DeleteAsync(lesonId), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_ReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var lesonId = _fixture.Create<int>();

        _lessonServiceMock.Setup(srv => srv.DeleteAsync(lesonId))
            .ReturnsAsync(false);

        // Act
        var result = await _lessonController.Delete(lesonId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _lessonServiceMock.Verify(srv => srv.DeleteAsync(lesonId), Times.Once);
    }
}

