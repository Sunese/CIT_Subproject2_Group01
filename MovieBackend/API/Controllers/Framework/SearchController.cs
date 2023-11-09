using System.Security.Claims;
using API.Security;
using Application.Enums;
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

    [HttpGet("title", Name = nameof(TitleSearch))]
    [Authorize]
    public IActionResult TitleSearch(string query, TitleType? titleType, int page = 0, int pageSize = 10)
    {
        var username = HttpContext.User.Identity.Name;
        var (searchResult, total) = _searchService.TitleSearch(username, query, titleType, page, pageSize);
        var items = searchResult.Select(result => new
        {
            Url = GetUrl("GetTitle", new { id = result.TitleID }),
            PrimaryTitle = result.PrimaryTitle
        });
        return Ok(Paging(items, total, page, pageSize, nameof(TitleSearch)));
    }

    [HttpGet("name", Name = nameof(NameSearch))]
    [Authorize]
    public IActionResult NameSearch(string query, int page = 0, int pageSize = 10)
    {
        var username = HttpContext.User.Identity.Name;
        var (searchResult, total) = _searchService.NameSearch(username, query, page, pageSize);
        var items = searchResult.Select(result => new
        {
            Url = GetUrl("GetName", new { id = result.NameId }),
            PrimaryName = result.PrimaryName
        });
        return Ok(Paging(items, total, page, pageSize, nameof(NameSearch)));
    }

    // Find actors by name
    [HttpGet("actor", Name = nameof(ActorSearch))]
    [Authorize]
    public IActionResult ActorSearch(string query, int page = 0, int pageSize = 10)
    {
        var username = HttpContext.User.Identity.Name;
        var (searchResult, total) = _searchService.FindActors(username, query, page, pageSize);
        var items = searchResult.Select(result => new
        {
            Url = GetUrl("GetName", new { id = result.NameId }),
            PrimaryName = result.PrimaryName
        });
        return Ok(Paging(items, total, page, pageSize, nameof(ActorSearch)));
    }

    // Find writers by name
    [HttpGet("writer", Name = nameof(WriterSearch))]
    [Authorize]
    public IActionResult WriterSearch(string query, int page = 0, int pageSize = 10)
    {
        var username = HttpContext.User.Identity.Name;
        var (searchResult, total) = _searchService.FindWriters(username, query, page, pageSize);
        var items = searchResult.Select(result => new
        {
            Url = GetUrl("GetName", new { id = result.NameId }),
            PrimaryName = result.PrimaryName
        });
        return Ok(Paging(items, total, page, pageSize, nameof(WriterSearch)));
    }

    // Find co-players by name
    [HttpGet("coplayer", Name = nameof(CoPlayerSearch))]
    [Authorize]
    public IActionResult CoPlayerSearch(string query, int page = 0, int pageSize = 10)
    {
        var username = HttpContext.User.Identity.Name;
        var (searchResult, total) = _searchService.FindCoPlayers(username, query, page, pageSize);
        var items = searchResult.Select(result => new
        {
            Url = GetUrl("GetName", new { id = result.NameId }),
            PrimaryName = result.PrimaryName
        });
        return Ok(Paging(items, total, page, pageSize, nameof(CoPlayerSearch)));
    }
}