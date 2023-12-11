using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Application.Context;
using Application.Models;
using Application.Profiles;
using Application.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntegrationTests;

public class MovieApiTests
{
    // NOTE: tests rely on the API project running separately
    private const string titlesApi = "https://localhost:7293/api/v1/title/";
    private const string namesApi = "https://localhost:7293/api/v1/name/";
    
    [Theory]
    [InlineData(titlesApi)]
    [InlineData(namesApi)]
    public async Task ApiResource_NoIdProvided_ReturnsPagedResultWithNextPageLink(string url)
    {
        var (json, status) = await GetObject(url);
        Assert.Equal(HttpStatusCode.OK, status);
        Assert.NotNull(json?["total"]);
        Assert.True(json?["total"].GetValue<int>() > 0);
        Assert.NotNull(json?["numberOfPages"]);
        Assert.NotNull(json?["next"]);
        Assert.Null(json?["prev"]);
        Assert.NotNull(json?["current"]);
        Assert.NotNull(json?["items"]);
    }

    [Theory]
    [InlineData(titlesApi + "tt12330546")]
    public async Task ApiGetTitle_ValidId_ReturnsTitleDTO(string url)
    {
        var (json, status) = await GetObject(url);
        Assert.Equal(HttpStatusCode.OK, status);
        Assert.NotNull(json);
        Assert.Equal("tt12330546", json["titleID"].GetValue<string>());
    }

    [Theory]
    [InlineData(namesApi + "nm0000001")]
    public async Task ApiGetName_ValidId_ReturnsNameDTO(string url)
    {
        var (json, status) = await GetObject(url);
        Assert.Equal(HttpStatusCode.OK, status);
        Assert.NotNull(json);
        Assert.Equal("nm0000001", json["nameID"].GetValue<string>());
    }

    [Theory]
    [InlineData(namesApi + "nm99999999999")]
    public async Task ApiGetName_NonExistingId_Returns404(string url)
    {
        var (json, status) = await GetObject(url);
        Assert.Equal(HttpStatusCode.NotFound, status);
    }

    [Theory]
    [InlineData(titlesApi + "tt99999999999")]
    public async Task ApiGetTitle_NonExistingId_Returns404(string url)
    {
        var (json, status) = await GetObject(url);
        Assert.Equal(HttpStatusCode.NotFound, status);
    }

    async Task<(JsonObject?, HttpStatusCode)> GetObject(string url)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }
}