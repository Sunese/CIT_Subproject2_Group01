using System.Security.Claims;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.Framework;

[ApiController]
[Route("api/v1/search")]
public class SearchController : MovieBaseController
{
    private readonly IAccountService _accountService;
    private readonly IBookmarkService _bookmarkService;
    private readonly ISearchService _searchService;

    public SearchController(
        IAccountService accountService,
        IBookmarkService bookmarkService,
        ISearchService searchService,
        LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _accountService = accountService;
        _bookmarkService = bookmarkService;
        _searchService = searchService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Search([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.Search(username, query));
    }

    [HttpGet("title")]
    [Authorize]
    public IActionResult TitleSearch([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.TitleSearch(username, query));
    }

    [HttpGet("name")]
    [Authorize]
    public IActionResult NameSearch([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.NameSearch(username, query));
    }

    // Find actors by name
    [HttpGet("actor")]
    [Authorize]
    public IActionResult ActorSearch([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.FindActors(username, query)); // Use SP
    }

    // Find writers by name
    [HttpGet("writer")]
    [Authorize]
    public IActionResult WriterSearch([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.FindWriters(username, query)); // Use SP
    }

    // Find co-players by name
    [HttpGet("coplayer")]
    [Authorize]
    public IActionResult CoPlayerSearch([FromQuery] string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.FindCoPlayers(username, query)); // Use SP
    }
}