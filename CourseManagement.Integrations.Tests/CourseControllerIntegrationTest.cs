using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace CourseManagement.Integrations.Tests;
public class CourseControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CourseControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task GetAll_Should_ReturnOk_WhenCoursesExists()
    {
        // Arrange

        // Act

        // Assert
    }
}

