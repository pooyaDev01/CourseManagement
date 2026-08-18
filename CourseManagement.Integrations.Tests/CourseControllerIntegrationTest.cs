using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using System.Net;
using CourseManagement.Application.DTOs.Course;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Integrations.Tests;
public class CourseControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    private readonly CustomWebApplicationFactory _factory;

    public CourseControllerIntegrationTest(CustomWebApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }
    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenCoursesExists()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        // Act
        HttpResponseMessage responseMessage = await _client.GetAsync("/api/course");

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
    public async Task Add_Should_ReturnCreated_WhenCourseIsValid()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var request = new AddCourseRequestDto
        {
            Title = "Integration Test Course",
            Description = "Created By Integration Test",
            Price = 100
        };

        // Act
        var responseMessage = await _client.PostAsJsonAsync("api/course", request);

        // Get the response content for debugging
        var content = await responseMessage.Content.ReadAsStringAsync();

        // Log
        _output.WriteLine($"Status Code: {responseMessage.StatusCode}");
        _output.WriteLine($"Status Code Number: {(int)responseMessage.StatusCode}");
        _output.WriteLine($"Response Content: {content}");
        _output.WriteLine($"Is Success: {responseMessage.IsSuccessStatusCode}");

        // Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Add_Should_ReturnCreatedMultiCourses_WhenCourseIsValid()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var request1 = new AddCourseRequestDto
        {
            Title = "Course 1",
            Description = "First Course",
            Price = 100
        };

        var request2 = new AddCourseRequestDto
        {
            Title = "Course 2",
            Description = "Second Course",
            Price = 200
        };

        // Act
        var responseMessage1 = await _client.PostAsJsonAsync("api/course", request1);

        var responseMessage2 = await _client.PostAsJsonAsync("api/course", request2);

        // Get the response content for debugging
        var content1 = await responseMessage1.Content.ReadAsStringAsync();
        var content2 = await responseMessage2.Content.ReadAsStringAsync();

        // Log 1
        _output.WriteLine($"Status Code: {responseMessage1.StatusCode}");
        _output.WriteLine($"Status Code Number: {(int)responseMessage1.StatusCode}");
        _output.WriteLine($"Response Content: {content1}");
        _output.WriteLine($"Is Success: {responseMessage1.IsSuccessStatusCode}");

        // Log 2
        _output.WriteLine($"Status Code: {responseMessage2.StatusCode}");
        _output.WriteLine($"Status Code Number: {(int)responseMessage2.StatusCode}");
        _output.WriteLine($"Response Content: {content2}");
        _output.WriteLine($"Is Success: {responseMessage2.IsSuccessStatusCode}");

        // Assert
        responseMessage1.StatusCode.Should().Be(HttpStatusCode.Created);
        responseMessage2.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenCourseExists()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        
        var request = new AddCourseRequestDto
        {
            Title = "course1",
            Description = "course1 description",
            Price = 100
        };

        var createdCourse = await _client.PostAsJsonAsync("api/course", request);

        var contentOfCreatedCourse = await createdCourse.Content.ReadFromJsonAsync<CourseResponseDto>();
        var contentOfCreatedCourse2 = await createdCourse.Content.ReadAsStringAsync();

        // Log
        _output.WriteLine($"Craeted Content: {contentOfCreatedCourse2}");

        // Act
        var getCourse = await _client.GetAsync($"api/course/{contentOfCreatedCourse!.Id}");

        // Log
        _output.WriteLine("Status Code: " + getCourse.StatusCode);
        _output.WriteLine("Status Code Number: " + (int)getCourse.StatusCode);
        _output.WriteLine("Get Content: " + await getCourse.Content.ReadAsStringAsync());
        _output.WriteLine("Is Success: " + getCourse.IsSuccessStatusCode);

        // Assert
        getCourse.IsSuccessStatusCode.Should().BeTrue();
        getCourse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        int id = 1;

        // Act
        var responseMessage = await _client.GetAsync($"api/course/{id}");

        // Log
        _output.WriteLine("Status Code: " + responseMessage.StatusCode);
        _output.WriteLine("Status Code: " + (int)responseMessage.StatusCode);
        _output.WriteLine("Is Success: " + responseMessage.IsSuccessStatusCode);

        // Assert
        responseMessage.IsSuccessStatusCode.Should().BeFalse();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldUpdateSuccessfully_WhenCourseExists()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        var createRequest = new AddCourseRequestDto
        {
            Title = "course 1",
            Description = "course 1 description",
            Price = 100
        };

        var createResponse = await _client.PostAsJsonAsync("api/course", createRequest);

        var createJsonResponse = await createResponse.Content.ReadFromJsonAsync<CourseResponseDto>();

        var id = createJsonResponse!.Id;

        var Createcontent = await createResponse.Content.ReadAsStringAsync();

        // Log
        _output.WriteLine("Created Content: " + Createcontent);

        var updaterequest = new UpdateCourseRequestDto
        {
            Title = "Modified Title",
            Description = "Modified Description",
            Price = 200
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"api/course/{id}", updaterequest);

        var getAll = await _client.GetAsync("api/course");

        // Log
        _output.WriteLine("Status Code: " + updateResponse.StatusCode);
        _output.WriteLine("Status Code: " + (int)updateResponse.StatusCode);
        _output.WriteLine("Update Content: " + await getAll.Content.ReadAsStringAsync()); // if you log the update since it's response is no-content ,so it's log will be empty
        _output.WriteLine("Is Success: " + updateResponse.IsSuccessStatusCode);

        // Assert
        updateResponse.IsSuccessStatusCode.Should().BeTrue();
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange - Reset database for fresh test
        await _factory.ResetDatabaseAsync();

        int id = 1;

        var request = new UpdateCourseRequestDto
        {
            Title = "Modified Title",
            Description = "Modified Description",
            Price = 200
        };

        // Act
        var responseMessage = await _client.PutAsJsonAsync($"api/course/{id}", request);

        // Log
        _output.WriteLine("Status Code: " + responseMessage.StatusCode);
        _output.WriteLine("Status Code: " + (int)responseMessage.StatusCode);
        _output.WriteLine("Is Success: " + responseMessage.IsSuccessStatusCode);

        // Assert
        responseMessage.IsSuccessStatusCode.Should().BeFalse();
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

