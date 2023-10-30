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
[Route("[controller]")]
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
        if (!IsAuthorizedToUpdate(username))
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
} 