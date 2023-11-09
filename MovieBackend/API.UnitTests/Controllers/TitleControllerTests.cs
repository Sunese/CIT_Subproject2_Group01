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

    // Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    [Fact]
    public void GetTitles_ValidParams_ReturnsOKWithPagedTitles()
    {
        // Arrange
        var returnedTitleDTOs = new List<TitleDTO>() 
        {
            new TitleDTO {TitleID = "tt0000001", PrimaryTitle = "TestTitle1"},
            new TitleDTO {TitleID = "tt0000002", PrimaryTitle = "TestTitle2"},
            new TitleDTO {TitleID = "tt0000003", PrimaryTitle = "TestTitle3"},
            new TitleDTO {TitleID = "tt0000004", PrimaryTitle = "TestTitle4"},
            new TitleDTO {TitleID = "tt0000005", PrimaryTitle = "TestTitle5"}
        };
        var logger = new Mock<ILogger<TitleController>>();
        var mockTitleService = new Mock<ITitleService>();
        mockTitleService.Setup(x => x.GetTitles(It.IsAny<DateOnly>(), 
                                    It.IsAny<DateOnly>(), 
                                    It.IsAny<int>(),
                                    It.IsAny<int>(),
                                    It.IsAny<bool>()))
            .Returns((returnedTitleDTOs, returnedTitleDTOs.Count()));
        ITitleService titleService = mockTitleService.Object;

        var returnedUri = "http://localhost:5000/api/v1/title/testTitleId";
        var linkGenerator = new LinkGenerator();
        var mockLinkGenerator = new Mock<LinkGenerator>();
        mockLinkGenerator.Setup(x => x.GetUriByName(It.IsAny<HttpContext>(), 
                                                    It.IsAny<string>(), 
                                                    It.IsAny<object>(),
                                                    It.IsAny<string>(),
                                                    It.IsAny<HostString>(),
                                                    It.IsAny<string>(),
                                                    It.IsAny<FragmentString>(),
                                                    It.IsAny<LinkOptions>()))
            .Returns(returnedUri);

        var titleController = new TitleController(logger.Object, titleService, mockLinkGenerator.Object);

        // Act
        var result = titleController.GetTitles(null, null, null, null, null, null, true);

        // Assert
        Assert.NotNull(result);
        var okObject = result as OkObjectResult;
        Assert.NotNull(okObject);
        Assert.Equal(StatusCodes.Status200OK, okObject.StatusCode);
        var pagedTitles = okObject.Value as IDictionary<string, object>;
        Assert.NotNull(pagedTitles);
        Assert.Equal(5, pagedTitles["Total"]);
        Assert.Equal(1, pagedTitles["NumberOfPages"]);
        Assert.Null(pagedTitles["Next"]);
        Assert.Null(pagedTitles["Prev"]);
        Assert.Equal(returnedUri, pagedTitles["Current"]);

    }
    
}