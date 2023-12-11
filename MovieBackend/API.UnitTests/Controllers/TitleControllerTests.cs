using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Controllers.Movie;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.UnitTests.Controllers;

public class TitleControllerTests
{
    public static List<TitleDTO> TitleDTOs = new()
    {
        new() {TitleID = "tt0000001", PrimaryTitle = "TestTitle1"},
        new() {TitleID = "tt0000002", PrimaryTitle = "TestTitle2"},
        new() {TitleID = "tt0000003", PrimaryTitle = "TestTitle3"},
        new() {TitleID = "tt0000004", PrimaryTitle = "TestTitle4"},
        new() {TitleID = "tt0000005", PrimaryTitle = "TestTitle5"},
        new() {TitleID = "tt0000006", PrimaryTitle = "TestTitle6"}
    };

    public static string LinkGeneratorUri = "http://localhost:5000/api/v1/title/testTitleID";
    public static LinkGenerator MockLinkGenerator = new MockLinkGenerator(LinkGeneratorUri);

    // Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    [Fact]
    public void GetTitles_ValidParams_ReturnsOKWithPagedTitles()
    {
        //
        // ***Arrange***
        //
        var pageSize = 3;
        var page = 0;
        var pagedTitles = TitleDTOs.Skip(page * pageSize).Take(pageSize).ToList();
        var logger = new Mock<ILogger<TitleController>>();
        var mockTitleService = new Mock<ITitleService>();
        // Set up a mock TitleService that always returns the hard-coded list of TitleDTOs above
        // when GetTitles() is called
        mockTitleService.Setup(x => x.GetTitles(It.IsAny<DateOnly>(), 
                                    It.IsAny<DateOnly>(), 
                                    It.IsAny<int>(),
                                    It.IsAny<int>(),
                                    It.IsAny<bool>()))
            .Returns((pagedTitles, TitleDTOs.Count));
        var titleService = mockTitleService.Object;
        var titleController = new TitleController(logger.Object, titleService, MockLinkGenerator);
        titleController.ControllerContext = new ControllerContext();
        titleController.ControllerContext.HttpContext = new DefaultHttpContext();

        //
        // ***Act***
        //
        var result = titleController.GetTitles(null, null, null, null, null, null, true, pageSize: 3, page: 0);

        //
        // ***Assert***
        //
        Assert.NotNull(result);
        var okObject = result as OkObjectResult;
        Assert.NotNull(okObject);
        Assert.Equal(StatusCodes.Status200OK, okObject.StatusCode);
        // A small hack: RouteValueDictionary has a constructor that takes an object type, allowing
        // us to take any anonymous type and convert it into a dictionary
        var pairs = new RouteValueDictionary(okObject.Value);
        Assert.Equal(6, pairs["Total"]);
        Assert.Equal(LinkGeneratorUri, pairs["Next"]);
        Assert.Null(pairs["Prev"]);
        Assert.Equal(LinkGeneratorUri, pairs["Current"]);
        var searchPageItems = pairs["Items"] as IEnumerable<object>;
        Assert.NotNull(searchPageItems);
        Assert.Equal(3, searchPageItems.Count()); // Because page size is 3
        var searchPageItem = new RouteValueDictionary(searchPageItems.First()); 
        Assert.Equal(LinkGeneratorUri, searchPageItem["Url"]);
        Assert.Equal(TitleDTOs.First().PrimaryTitle, searchPageItem["Name"]);
        Assert.Equal(TitleDTOs.First().Released, searchPageItem["Released"]);
        Assert.Equal(TitleDTOs.First().Poster, searchPageItem["Poster"]);
    }
}

public class MockLinkGenerator : LinkGenerator
{
    private string uri;
    public MockLinkGenerator(string uri)
    {
        this.uri = uri;
    }

    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values,
        RouteValueDictionary? ambientValues = null, PathString? pathBase = null,
        FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        return uri;
    }

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values,
        PathString pathBase = new PathString(), FragmentString fragment = new FragmentString(),
        LinkOptions? options = null)
    {
        return uri;
    }

    public override string? GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values,
        RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null,
        PathString? pathBase = null, FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        return uri;
    }

    public override string? GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string? scheme, HostString host,
        PathString pathBase = new PathString(), FragmentString fragment = new FragmentString(),
        LinkOptions? options = null)
    {
        return uri;
    }
}