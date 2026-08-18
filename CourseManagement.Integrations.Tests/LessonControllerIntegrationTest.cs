using Azure.Core;
using CourseManagement.Application.DTOs.Course;
using CourseManagement.Application.DTOs.Lesson;
using CourseManagement.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;

namespace CourseManagement.Integrations.Tests;

public class LessonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    private readonly CustomWebApplicationFactory _factory;
    public LessonControllerIntegrationTest(CustomWebApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;

    }

    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenLessonsExists()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        // Act
        HttpResponseMessage responseMessage = await _client.GetAsync("/api/lesson");

        // Get the response content for debugging
        var content = await responseMessage.Content.ReadAsStringAsync();

        // Log the response details using ITestOutputHelper
        _output.WriteLine($"Status Code: {responseMessage.StatusCode}");
        _output.WriteLine($"Status Code Number: {(int)responseMessage.StatusCode}");
        _output.WriteLine($"Response Content: {content}");
        _output.WriteLine($"Is Success: {responseMessage.IsSuccessStatusCode}");

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        responseMessage.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Add_Should_ReturnCreated_WhenLessonIsValid()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var courseRequest = new AddCourseRequestDto
        {
            Title = "Course",
            Description = "Course Description",
            Price = 100
        };
     
        var course = await _client.PostAsJsonAsync("api/course", courseRequest);

        var courseContent = await course.Content.ReadFromJsonAsync<CourseResponseDto>();

        var courseId = courseContent!.Id;

        var lessonAddRequest = new AddLessonRequestDto
        {
            Title = "lesson",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        // Act
        var lessonAddResponse = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest);

        // Assert
        lessonAddResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        lessonAddResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Add_Should_ReturnCreatedMultiLessons_WhenLessonIsValid()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var courseRequest = new AddCourseRequestDto
        {
            Title = "Course",
            Description = "Course Description",
            Price = 100
        };

        var course = await _client.PostAsJsonAsync("api/course", courseRequest);

        var courseContent = await course.Content.ReadFromJsonAsync<CourseResponseDto>();

        var courseId = courseContent!.Id;

        var lessonAddRequest1 = new AddLessonRequestDto
        {
            Title = "lesson1",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        var lessonAddRequest2 = new AddLessonRequestDto
        {
            Title = "lesson2",
            CourseId = courseId,
            DurationInMinutes = 35
        };

        // Act
        var lessonAddResponse1 = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest1);

        var lessonAddResponse2 = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest2);

        // Assert
        lessonAddResponse1.StatusCode.Should().Be(HttpStatusCode.Created);
        lessonAddResponse1.IsSuccessStatusCode.Should().BeTrue();
        lessonAddResponse2.StatusCode.Should().Be(HttpStatusCode.Created);
        lessonAddResponse2.IsSuccessStatusCode.Should().BeTrue();
    }

    //whencourseisnotvalid

    [Fact]
    public async Task Add_Should_ReturnNotFound_WhenCourseIdIsNotValid()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var courseId = 10;

        var lessonAddRequest = new AddLessonRequestDto
        {
            Title = "lesson",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        // Act
        var lessonAddResponse = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest);

        // Assert
        lessonAddResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        lessonAddResponse.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenLessonExists()
    {
        await _factory.ResetDatabaseAsync();

        var courseRequest = new AddCourseRequestDto
        {
            Title = "Course",
            Description = "Course Description",
            Price = 100
        };

        var course = await _client.PostAsJsonAsync("api/course", courseRequest);

        var courseContent = await course.Content.ReadFromJsonAsync<CourseResponseDto>();

        var courseId = courseContent!.Id;

        var lessonAddRequest = new AddLessonRequestDto
        {
            Title = "lesson",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        var lessonAddResponse = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest);

        var lessonContent = await lessonAddResponse.Content.ReadFromJsonAsync<LessonResponseDto>();

        // Act
        var getLesson = await _client.GetAsync($"api/lesson/{lessonContent!.Id}");

        // Assert
        getLesson.IsSuccessStatusCode.Should().BeTrue();
        getLesson.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        int id = 1;

        // Act
        var responseMessage = await _client.GetAsync($"api/lesson/{id}");

        // Assert
        responseMessage.IsSuccessStatusCode.Should().BeFalse();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldUpdateSuccessfully_WhenLessonExists()
    {
        await _factory.ResetDatabaseAsync();

        var courseRequest = new AddCourseRequestDto
        {
            Title = "Course",
            Description = "Course Description",
            Price = 100
        };

        var course = await _client.PostAsJsonAsync("api/course", courseRequest);

        var courseContent = await course.Content.ReadFromJsonAsync<CourseResponseDto>();

        var courseId = courseContent!.Id;

        var lessonAddRequest = new AddLessonRequestDto
        {
            Title = "lesson",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        var lessonAddResponse = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest);

        var lessonContent = await lessonAddResponse.Content.ReadFromJsonAsync<LessonResponseDto>();

        var updaterequest = new UpdateLessonRequestDto
        {
            Title = "Modified Title",
            CourseId = courseId,
            DurationInMinutes = 45
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/lesson/{lessonContent!.Id}", updaterequest);

        // Assert
        updateResponse.IsSuccessStatusCode.Should().BeTrue();
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        int id = 1;

        var updaterequest = new UpdateLessonRequestDto
        {
            Title = "Modified Title",
            CourseId = id,
            DurationInMinutes = 45
        };

        // Act
        var responseMessage = await _client.PutAsJsonAsync($"api/lesson/{id}", updaterequest);

        // Assert
        responseMessage.IsSuccessStatusCode.Should().BeFalse();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ShouldDeleteSuccessfully_WhenLessonExists()
    {
        await _factory.ResetDatabaseAsync();

        var courseRequest = new AddCourseRequestDto
        {
            Title = "Course",
            Description = "Course Description",
            Price = 100
        };

        var course = await _client.PostAsJsonAsync("api/course", courseRequest);

        var courseContent = await course.Content.ReadFromJsonAsync<CourseResponseDto>();

        var courseId = courseContent!.Id;

        var lessonAddRequest = new AddLessonRequestDto
        {
            Title = "lesson",
            CourseId = courseId,
            DurationInMinutes = 15
        };

        var lessonAddResponse = await _client.PostAsJsonAsync("api/lesson", lessonAddRequest);

        var lessonContent = await lessonAddResponse.Content.ReadFromJsonAsync<LessonResponseDto>();

        // Act
        var responseMessage = await _client.DeleteAsync($"api/lesson/{lessonContent!.Id}");

        // Assert
        responseMessage.IsSuccessStatusCode.Should().BeTrue();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        int id = 1;

        // Act
        var responseMessage = await _client.DeleteAsync($"api/lesson/{id}");


        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseMessage.IsSuccessStatusCode.Should().BeFalse();

    }
}

