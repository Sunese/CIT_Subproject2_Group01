using Application.Context;
using Application.Models;
using Application.Profiles;
using Application.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

public class WebApplicationFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WebApplicationFactoryTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7
    [Theory]
    [InlineData("https://localhost:7239/api/v1/title")]
    [InlineData("https://localhost:7239/api/v1/name")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}