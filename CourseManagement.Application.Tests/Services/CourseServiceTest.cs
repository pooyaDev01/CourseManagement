using AutoFixture;
using AutoMapper;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Interfaces.Services;
using CourseManagement.Application.Mappings;
using CourseManagement.Application.Services;
using CourseManagement.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Tests.Services;
public class CourseServiceTest
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CourseService _courseService; //This shows our SUT(System Under Test) here is the CourseService class
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public CourseServiceTest()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }
        ,NullLoggerFactory.Instance);

        _mapper = mapperConfig.CreateMapper();

        _courseService = new CourseService(_courseRepositoryMock.Object, _mapper);

        _fixture = new Fixture();

    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnCourse_WhenCourseExists()
    {
        // Arrange
        var course = _fixture.Build<Course>()
            //.With(c => c.Lessons, null as List<Lesson>)
            .Without(c => c.Lessons)
            .Create();

        _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(course.Id))
             .ReturnsAsync(course);

        //Act
        var result = await _courseService.GetByIdAsync(course.Id);

        //Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(course, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_WhenCourseDoesNotExists()
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.GetByIdAsync(courseId);

        // Assert
        result.Should().BeNull();

        _courseRepositoryMock.Verify(x => x.GetByIdAsync(courseId), Times.Once);

    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllCourses_WhenCoursesExists()
    {
        // Arrange
        var Courses = _fixture.Build<Course>()
            .Without(c => c.Lessons)
            .CreateMany<Course>()
            .ToList();

        _courseRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Courses);

        // Act
        var result = await _courseService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(Courses, options => options.ExcludingMissingMembers());
        _courseRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }


}

