using System.Net;
using System.Net.Http.Json;
using System.Text;
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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntegrationTests;

public class FrameworkApiTests
{
    // NOTE: tests rely on the API project running separately
    // NOTE: depends on a user called 'testuser' with password 'testpassword'
    //       and user called 'testuser2' with password 'testpassword2' being created
    private const string accountApi = "https://localhost:7293/api/v1/account/";
    private const string api = "https://localhost:7293/api/v1/";

    [Theory]
    [InlineData(accountApi + "login")]
    public async Task Login_CorrectCredentials_ReturnsJwtToken(string url)
    {
        var signInModel = new 
        {
            username = "testuser",
            password = "testpassword"
        };
        var (json, status) = await PostObject(url, signInModel);
        Assert.Equal(HttpStatusCode.OK, status);
        Assert.NotNull(json);
        Assert.StartsWith("Bearer ey", json["token"].GetValue<string>());
    }

    [Theory]
    [InlineData(accountApi + "login")]
    public async Task Login_BadCredentials_Unauthorized(string url)
    {
        var signInModel = new 
        {
            username = "testuser",
            password = "notthepassword"
        };
        var (json, status) = await PostObject(url, signInModel);

        Assert.Equal(HttpStatusCode.Unauthorized, status);
    }

    // Signed in user can access their bookmarks
    [Theory]
    [InlineData(api + "testuser/" + "titlebookmark")]
    public async Task GetTitleBookmarks_SignedIn_Ok(string url)
    {
        // sign in as 'testuser'
        var signInModel = new 
        {
            username = "testuser",
            password = "testpassword"
        };
        var (json, status) = await PostObject(accountApi + "login", signInModel);
        var token = json["token"].GetValue<string>();
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", token);
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    // Users cannot access other users' bookmarks
    [Theory]
    [InlineData(api + "testuser2/" + "titlebookmark")]
    public async Task GetTitleBookmarks_NotSignedIn_Unauthorized(string url)
    {
        // sign in as 'testuser'
        var signInModel = new 
        {
            username = "testuser",
            password = "testpassword"
        };
        var (json, status) = await PostObject(accountApi + "login", signInModel);
        var token = json["token"].GetValue<string>();
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", token);
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }



    async Task<(JsonObject?, HttpStatusCode)> GetObject(string url)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }

    async Task<(JsonObject?, HttpStatusCode)> PostObject(string url, object content)
    {
        var client = new HttpClient();
        var requestContent = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");
        var response = await client.PostAsync(url, requestContent);
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }
}