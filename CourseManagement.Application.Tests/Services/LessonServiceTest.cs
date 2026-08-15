using AutoFixture;
using AutoMapper;
using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Application.Interfaces.Repositories;
using CourseManagement.Application.Mappings;
using CourseManagement.Application.Services;
using CourseManagement.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Tests.Services;

public class LessonServiceTest
{
    private readonly Mock<ILessonRepository> _lessonRepositoryMock;
    private readonly LessonService _lessonService;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public LessonServiceTest()
    {
        _lessonRepositoryMock = new Mock<ILessonRepository>();

        MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }
        ,NullLoggerFactory.Instance);

        _mapper = mapConfig.CreateMapper();

        _lessonService = new LessonService(_lessonRepositoryMock.Object, _mapper);

        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnLesson_WhenLessonExists()
    {
        // Arrange
        var lesson = _fixture.Build<Lesson>()
            .Without(l => l.Course)
            .Create();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lesson.Id))
            .ReturnsAsync(lesson);

        // Act
        var result = await _lessonService.GetByIdAsync(lesson.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(lesson, options => options.ExcludingMissingMembers());
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lesson.Id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_WhenLessonDoesNotExist()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lessonId))
            .ReturnsAsync((Lesson?)null);

        // Act
        var result = await _lessonService.GetByIdAsync(lessonId);

        // Assert
        result.Should().BeNull();
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lessonId), Times.Once);

    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllLessons()
    {
        // Arrange
        var lessons = _fixture.Build<Lesson>()
            .Without(l => l.Course)
            .CreateMany(3)
            .ToList();

        _lessonRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(lessons);

        // Act
        var result = await _lessonService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(lessons, options => options.ExcludingMissingMembers());
        _lessonRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task AddAsync_Should_CreateAndReturnLesson()
    {
        // Arrange
        var dto = _fixture.Create<AddLessonRequestDto>();

        var expectedLesson = new Lesson
        {
            Title = dto.Title,
            DurationInMinutes = dto.DurationInMinutes,
            CourseId = dto.CourseId
        };

        _lessonRepositoryMock.Setup(repo => repo.AddAsync(It.Is<Lesson>(lesson =>
            lesson.Title == dto.Title &&
            lesson.DurationInMinutes == dto.DurationInMinutes &&
            lesson.CourseId == dto.CourseId
        ))).ReturnsAsync(expectedLesson);

        // Act
        var result = await _lessonService.AddAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedLesson, options => options.ExcludingMissingMembers());
        _lessonRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Lesson>(lesson =>
            lesson.Title == dto.Title &&
            lesson.DurationInMinutes == dto.DurationInMinutes &&
            lesson.CourseId == dto.CourseId
            )), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNull_WhenLessonDoesNotExist()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        var dto = _fixture.Create<UpdateLessonRequestDto>();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lessonId))
            .ReturnsAsync((Lesson?)null);

        // Act
        var result = await _lessonService.UpdateAsync(lessonId, dto);

        // Assert
        result.Should().BeNull();
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lessonId), Times.Once);
        _lessonRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Lesson>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateSuccessfully_WhenLessonExists()
    {
        // Arrange
        var lesson = _fixture.Build<Lesson>()
            .Without(l => l.Course)
            .Create();

        var dto = _fixture.Create<UpdateLessonRequestDto>();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lesson.Id))
            .ReturnsAsync(lesson);

        // Act
        var result =  await _lessonService.UpdateAsync(lesson.Id, dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(lesson, options => options.ExcludingMissingMembers()); // we used leeson to compare cuase we want to see, is lesson updated or not?!
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lesson.Id), Times.Once);
        _lessonRepositoryMock.Verify(repo => repo.UpdateAsync(lesson), Times.Once); // here we can say lesson as input paramter cause we map the dto to lesson in service implementation within in own reference with this map overload (Map(Dto , Domain))
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnFalse_WhenLessonDoesNotExist()
    {
        // Arrange
        var lessonId = _fixture.Create<int>();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lessonId))
            .ReturnsAsync((Lesson?)null);

        // Act
        var result = await _lessonService.DeleteAsync(lessonId);

        // Assert
        result.Should().BeFalse();
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lessonId), Times.Once);
        _lessonRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Lesson>()), Times.Never);

    }
    [Fact]
    public async Task DeleteAsync_Should_ReturnTrue_WhenLessonExists()
    {
        // Arrange
        var lesson = _fixture.Build<Lesson>()
            .Without(l => l.Course)
            .Create();

        _lessonRepositoryMock.Setup(repo => repo.GetByIdAsync(lesson.Id))
            .ReturnsAsync(lesson);
        // Act
        var result = await _lessonService.DeleteAsync(lesson.Id);

        // Assert
        result.Should().BeTrue();
        _lessonRepositoryMock.Verify(repo => repo.GetByIdAsync(lesson.Id), Times.Once);
        _lessonRepositoryMock.Verify(repo => repo.DeleteAsync(lesson), Times.Once);
    }
}

