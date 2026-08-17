using AutoFixture;
using CourseManagement.Api.Controllers;
using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Api.Tests;

public class CourseControllerTest
{
    private readonly Mock<ICourseService> _courseServiceMock;

    private readonly CourseController _courseController; //this is our SUT in this case

    private readonly Fixture _fixture;

    public CourseControllerTest()
    {
        _courseServiceMock = new Mock<ICourseService>();

        _courseController = new CourseController(_courseServiceMock.Object);

        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetById_Should_ReturnOk_WhenCourseExists() //Return 200
    {
        // Arrange
        var dto = _fixture.Create<CourseResponseDto>();

        _courseServiceMock.Setup(srv => srv.GetByIdAsync(dto.Id))
            .ReturnsAsync(dto);

        // Act
        var result = await _courseController.GetById(dto.Id);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeOfType<CourseResponseDto>();
        okResult.Value.Should().BeEquivalentTo(dto);
        _courseServiceMock.Verify(srv => srv.GetByIdAsync(dto.Id), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotFound_WhenCourseDoesNotExist() //Return 404
    {
        // Arrange
        var id = _fixture.Create<int>();

        _courseServiceMock.Setup(srv => srv.GetByIdAsync(id))
            .ReturnsAsync((CourseResponseDto?)null);

        // Act
        var result = await _courseController.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundResult>();
        _courseServiceMock.Verify(srv => srv.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenCoursesExists() //Return 200
    {
        // Arrange
        var courses = _fixture.CreateMany<CourseResponseDto>(10).ToList();

        _courseServiceMock.Setup(srv => srv.GetAllAsync())
            .ReturnsAsync(courses);

        // Act
        var result = await _courseController.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(courses);
        _courseServiceMock.Verify(srv => srv.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Add_Should_CreateAndReturnCourse() //Return 201
    {
        // Arrange
        var add_request_dto = _fixture.Create<AddCourseRequestDto>();

        var expectedResponse = new CourseResponseDto
        {
            Title = add_request_dto.Title,
            Description = add_request_dto.Description,
            Price = add_request_dto.Price
        };

        _courseServiceMock.Setup(srv => srv.AddAsync(add_request_dto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _courseController.Add(add_request_dto);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<CreatedAtActionResult>().Which.Value.Should().BeEquivalentTo(expectedResponse);
        _courseServiceMock.Verify(srv => srv.AddAsync(add_request_dto), Times.Once);
            
    }

    [Fact]
    public async Task Update_Should_UpdateSuccessfully_WhenCourseExists() //Return 204
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateCourseRequestDto>();

        var expectedResponse = new CourseResponseDto
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price
        };

        _courseServiceMock.Setup(srv => srv.UpdateAsync(courseId, dto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _courseController.Update(courseId, dto);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NoContentResult>();
        _courseServiceMock.Verify(srv => srv.UpdateAsync(courseId, dto));
    }

    [Fact]
    public async Task Update_Should_ReturnNotFound_WhenCourseDoesnNotExist() //Return 404
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateCourseRequestDto>();

        _courseServiceMock.Setup(srv => srv.UpdateAsync(courseId, dto))
            .ReturnsAsync((CourseResponseDto?)null);

        // Act
        var result = await _courseController.Update(courseId, dto);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundResult>();
        _courseServiceMock.Verify(srv => srv.UpdateAsync(courseId, dto));
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenCourseExists()
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        _courseServiceMock.Setup(srv => srv.DeleteAsync(courseId))
            .ReturnsAsync(true);

        // Act
        var result = await _courseController.Delete(courseId);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NoContentResult>();
        _courseServiceMock.Verify(srv => srv.DeleteAsync(courseId), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_ReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        _courseServiceMock.Setup(srv => srv.DeleteAsync(courseId))
            .ReturnsAsync(false);

        // Act
        var result = await _courseController.Delete(courseId);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundResult>();
        _courseServiceMock.Verify(srv => srv.DeleteAsync(courseId), Times.Once);
    }
}

