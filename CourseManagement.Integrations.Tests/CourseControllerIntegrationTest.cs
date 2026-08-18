using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using System.Net;

namespace CourseManagement.Integrations.Tests;
public class CourseControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public CourseControllerIntegrationTest(CustomWebApplicationFactory factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _output = output;
    }
    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenCoursesExists()
    {
        // Arrange

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
}

