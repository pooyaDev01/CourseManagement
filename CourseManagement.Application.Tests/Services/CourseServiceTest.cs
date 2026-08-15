using AutoFixture;
using AutoMapper;
using CourseManagement.Application.DTOs.Course;
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

        _courseRepositoryMock.Verify(x => x.GetByIdAsync(course.Id), Times.Once);
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

    [Fact]
    public async Task AddAsync_Should_CreateAndReturnCourse()
    {
        // Arrange
        var dto = _fixture.Create<AddCourseRequestDto>();

        // We will map dto to domain manually, to see does map works correctly or no in repo

        var expected_course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price
        };

        _courseRepositoryMock.Setup(repo => repo.AddAsync(It.Is<Course>(course =>
            course.Title == dto.Title &&
            course.Description == dto.Description &&
            course.Price == dto.Price)))
            .ReturnsAsync(expected_course);

        // Act
        var result = await _courseService.AddAsync(dto);

        // Assert
        result.Should().NotBeNull();

        // we will make sure the input parameter given to the repository is the exact values of dto while running in service
        result.Should().BeEquivalentTo(expected_course, options => options.ExcludingMissingMembers());
        _courseRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Course>(course =>
            course.Title == dto.Title &&
            course.Description == dto.Description &&
            course.Price == dto.Price)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNull_WhenCourseDoesntExist()
    {
        // Arrange
        var courseId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateCourseRequestDto>();

        _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.UpdateAsync(courseId, dto);

        // Arrest
        result.Should().BeNull();

        _courseRepositoryMock.Verify(repo => repo.GetByIdAsync(courseId), Times.Once);

        _courseRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Course>()), Times.Never); // we have to make sure it doesnt continue to reach to the update method in service
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateSuccessfully_WhenCourseExists()
    {

        // Arrange
        var course = _fixture.Build<Course>()
            .Without(c => c.Lessons)
            .Create();

        var dto = _fixture.Create<UpdateCourseRequestDto>();

        _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(course.Id))
            .ReturnsAsync(course);

        // Act
        var result = await _courseService.UpdateAsync(course.Id, dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CourseResponseDto>();
        result.Should().BeEquivalentTo(dto);
        _courseRepositoryMock.Verify(repo => repo.GetByIdAsync(course.Id), Times.Once);
        _courseRepositoryMock.Verify(repo => repo.UpdateAsync(course), Times.Once);
    }

}

