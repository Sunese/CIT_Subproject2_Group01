using System.Security.Claims;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[ApiController]
[Route("api/v1/search")]
public class SearchController : FrameworkBaseController
{
    private readonly IAccountService _accountService;
    private readonly IBookmarkService _bookmarkService;
    private readonly ISearchService _searchService;

    public SearchController(
        IAccountService accountService,
        IBookmarkService bookmarkService,
        ISearchService searchService)
    {
        _accountService = accountService;
        _bookmarkService = bookmarkService;
        _searchService = searchService;
    }

    [HttpGet("title")]
    [Authorize]
    public IActionResult TitleSearch([FromQuery]string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.TitleSearch(username, query));
    }

    [HttpGet("name")]
    [Authorize]
    public IActionResult NameSearch([FromQuery]string query)
    {
        var username = HttpContext.User.Identity.Name;
        return Ok(_searchService.NameSearch(username, query));
    }


}