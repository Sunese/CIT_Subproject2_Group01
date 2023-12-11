using System.Security.Claims;
using API.Models;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text.RegularExpressions;

namespace API.Controllers.Framework;

[ApiController]
[Route("api/v1")] // routes are defined in the methods, they depend on the username
public class BookmarkController : MovieBaseController
{
    private readonly IAccountService _userService;
    private readonly ITitleService _titleService;
    private readonly INameService _nameService;
    private readonly IMapper _mapper;
    private readonly IBookmarkService _bookmarkService;

    public BookmarkController(
        IAccountService userService,
        IBookmarkService bookmarkService,
        ITitleService titleService,
        INameService nameService,
        IMapper mapper,
        LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _mapper = mapper;
        _titleService = titleService;
        _userService = userService;
        _bookmarkService = bookmarkService;
        _nameService = nameService;
    }

    [HttpGet("{username}/titlebookmark/{titleID}", Name = nameof(GetTitleBookmark))]
    [Authorize]
    // NOTE: only the user can access their own bookmarks
    public IActionResult GetTitleBookmark(string username, string titleID)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_titleService.TitleExists(titleID, out var foundTitle))
        {
            return BadRequest("Title does not exist");
        }
        if (!_bookmarkService.TryGetTitleBookmark(username, titleID, out var foundBookmark))
        {
            return NotFound();
        }
        return Ok(CreateTitleBookmarkPageItem(foundBookmark));
    }

    [HttpGet("{username}/titlebookmark", Name = nameof(GetTitleBookmarks))]
    [Authorize]
    // NOTE: only the user can access their own bookmarks
    public IActionResult GetTitleBookmarks(string username,
                                           OrderBy orderBy = OrderBy.Alphabetical,
                                           int page = 0,
                                           int pageSize = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        var (bookmarks, total) = _bookmarkService
                                    .GetTitleBookmarks(username, orderBy, page, pageSize);
        var items = bookmarks.Select(CreateTitleBookmarkPageItem);
        return Ok(Paging(items,
                         total,
                         page,
                         pageSize,
                         nameof(GetTitleBookmarks),
                         new RouteValueDictionary { { "username", username } }));
    }

    [HttpPost("{username}/titlebookmark", Name = nameof(CreateTitleBookmark))]
    [Authorize]
    public IActionResult CreateTitleBookmark(
        string username,
        CreateTitleBookmarkModel model)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_titleService.TitleExists(model.TitleID, out var foundTitle))
        {
            return BadRequest("Title does not exist");
        }
        if (_bookmarkService.TitleBookmarkExists(username, model.TitleID))
        {
            return BadRequest("Title bookmark already exists");
        }
        var bookmarkDTO = _mapper.Map<TitleBookmarkDTO>(model);
        _bookmarkService.CreateTitleBookmark(username, bookmarkDTO);
        return CreatedAtAction(nameof(GetTitleBookmarks), new { username }, bookmarkDTO);
    }

    [HttpPatch("{username}/titlebookmark/{titleID}", Name = nameof(UpdateTitleBookmarkNote))]
    [Authorize]
    public IActionResult UpdateTitleBookmarkNote(
        string username,
        string titleID,
        [FromBody] UpdateBookmarkNote model)
    {
        if (isInValidNoteInput(model.Notes))
        {
            return BadRequest();
        }

        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _bookmarkService.UpdateTitleBookmarkNote(username, titleID, model.Notes);
        return Ok();
    }

    [HttpDelete("{username}/titlebookmark/{titleID}", Name = nameof(DeleteTitleBookmark))]
    [Authorize]
    public IActionResult DeleteTitleBookmark(
        string username,
        string titleID)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_titleService.TitleExists(titleID, out var foundTitle))
        {
            return BadRequest("Title does not exist");
        }
        if (!_bookmarkService.TryGetTitleBookmark(username, titleID, out var foundBookmark))
        {
            return BadRequest();
        }
        _bookmarkService.DeleteTitleBookmark(username, foundBookmark);
        return Ok();
    }

    [HttpGet("{username}/namebookmark/{nameID}", Name = nameof(GetNameBookmark))]
    [Authorize]
    // NOTE: only the user can access their own bookmarks
    public IActionResult GetNameBookmark(string username, string nameID)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_nameService.NameExists(nameID, out var foundName))
        {
            return BadRequest("Name does not exist");
        }
        if (!_bookmarkService.TryGetNameBookmark(username, nameID, out var foundBookmark))
        {
            return NotFound();
        }
        return Ok(CreateNameBookmarkPageItem(foundBookmark));
    }

    [HttpGet("{username}/namebookmark", Name = nameof(GetNameBookmarks))]
    [Authorize]
    // NOTE: only the user can access their own bookmarks
    public IActionResult GetNameBookmarks(string username, 
                                          OrderBy orderBy = OrderBy.Alphabetical, 
                                          int page = 0, 
                                          int pageSize = 10)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        var (bookmarks, total) = _bookmarkService
                                    .GetNameBookmarks(username, orderBy, page, pageSize);
        var items = bookmarks.Select(CreateNameBookmarkPageItem);
        return Ok(Paging(items,
                         total,
                         page,
                         pageSize,
                         nameof(GetNameBookmarks),
                         new RouteValueDictionary { { "username", username } }));
    }

    [HttpPost("{username}/namebookmark", Name = nameof(CreateNameBookmark))]
    [Authorize]
    public IActionResult CreateNameBookmark(
        string username,
        CreateNameBookmarkModel model)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (_bookmarkService.NameBookmarkExists(username, model.NameID))
        {
            return BadRequest("Name bookmark already exists");
        }
        var bookmarkDTO = _mapper.Map<NameBookmarkDTO>(model);
        _bookmarkService.CreateNameBookmark(username, bookmarkDTO);
        return CreatedAtAction(nameof(GetNameBookmarks), new { username }, bookmarkDTO);
    }

    [HttpPatch("{username}/namebookmark/{nameID}", Name = nameof(UpdateNameBookmarkNote))]
    [Authorize]
    public IActionResult UpdateNameBookmarkNote(
        string username,
        string nameID,
        [FromBody] UpdateBookmarkNote model)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }

        _bookmarkService.UpdateNameBookmarkNote(username, nameID, model.Notes);
        return Ok();
    }

    [HttpDelete("{username}/namebookmark/{nameid}", Name = nameof(DeleteNameBookmark))]
    [Authorize]
    public IActionResult DeleteNameBookmark(
        string username,
        string nameid)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_nameService.NameExists(nameid, out var foundName))
        {
            return BadRequest("Title does not exist");
        }
        if (!_bookmarkService.TryGetNameBookmark(username, nameid, out var foundBookmark))
        {
            return BadRequest();
        }
        _bookmarkService.DeleteNameBookmark(username, foundBookmark);
        return Ok();
    }

    private object CreateTitleBookmarkPageItem(TitleBookmarkDTO titleBookmarkDTO)
    {
        return new 
        {
            TitleID = titleBookmarkDTO.TitleID,
            Url = GetUrl("GetTitle", new { id = titleBookmarkDTO.TitleID }),
            Title = titleBookmarkDTO.Title,
            Notes = titleBookmarkDTO.Notes,
        };
    }

    private object CreateNameBookmarkPageItem(NameBookmarkDTO nameBookmarkDTO)
    {
        return new
        {
            NameID = nameBookmarkDTO.NameID,
            Url = GetUrl("GetName", new { id = nameBookmarkDTO.NameID }),
            Name = nameBookmarkDTO.Name,
            Notes = nameBookmarkDTO.Notes,
        };
    }

    private bool isInValidNoteInput(string noteInput)
    {
        if (!Regex.IsMatch(noteInput, "^[A-Za-z0-9.,' ]*$"))
        {
            return true;
        }
        return false;
    }
}