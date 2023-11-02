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
[Route("")] // routes are defined in the methods, they depend on the username
public class BookmarkController : FrameworkBaseController
{
    private readonly IUserService _userService;
    private readonly IBookmarkService _bookmarkService;

    public BookmarkController(
        IUserService userService,
        IBookmarkService bookmarkService)
    {
        _userService = userService;
        _bookmarkService = bookmarkService;
    }

    [HttpGet("{username}/titlebookmark")]
    [Authorize]
    public IActionResult GetTitleBookmarks(string username, OrderBy orderBy = OrderBy.Alphabetical, int count = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        var bookmarks = _bookmarkService.GetTitleBookmarks(username, orderBy, count);
        return Ok(bookmarks);
    }

    [HttpPost("{username}/titlebookmark")]
    [Authorize]
    public IActionResult CreateTitleBookmark(
        string username, 
        [FromBody] TitleBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.CreateTitleBookmark(username, model);
        return Ok(); // TODO: return HTTP 201 Created with Location header
                     // to where the new resource can be found,
                     // i.e. a GET method that is not implemented yet
                     // NOTE: maybe not location header? maybe just return
                     // an object with a URI property?
    }

    [HttpPut("{username}/titlebookmark")]
    [Authorize]
    public IActionResult UpdateTitleBookmarkNote(
        string username,
        [FromBody] TitleBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.UpdateTitleBookmarkNote(username, model);
        return Ok();
    }

    [HttpDelete("{username}/titlebookmark")]
    [Authorize]
    public IActionResult DeleteTitleBookmark(
        string username,
        [FromBody] TitleBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.DeleteTitleBookmark(username, model);
        return Ok();
    }

    [HttpGet("{username}/namebookmark")]
    [Authorize]
    public IActionResult GetNameBookmarks(string username, OrderBy orderBy = OrderBy.Alphabetical, int count = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        var bookmarks = _bookmarkService.GetNameBookmarks(username, orderBy, count);
        return Ok(bookmarks);
    }

    [HttpPost("{username}/namebookmark")]
    [Authorize]
    public IActionResult CreateNameBookmark(
        string username, 
        [FromBody] NameBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.CreateNameBookmark(username, model);
        return Ok(); // TODO: return HTTP 201 Created with Location header
                     // to where the new resource can be found,
                     // i.e. a GET method that is not implemented yet
                     // NOTE: maybe not location header? maybe just return
                     // an object with a URI property?
    }

    [HttpPut("{username}/namebookmark")]
    [Authorize]
    public IActionResult UpdateNameBookmarkNote(
        string username,
        [FromBody] NameBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.UpdateNameBookmarkNote(username, model);
        return Ok();
    }

    [HttpDelete("{username}/namebookmark")]
    [Authorize]
    public IActionResult DeleteNameBookmark(
        string username,
        [FromBody] NameBookmarkDTO model)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.DeleteNameBookmark(username, model);
        return Ok();
    }
}