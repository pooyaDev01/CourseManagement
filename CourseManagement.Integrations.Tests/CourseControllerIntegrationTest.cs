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
}

