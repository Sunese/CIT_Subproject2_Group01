using API.Models;
using Application.Models;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Framework;


[ApiController]
[Route("/api/v1")]
public class UserRatingController : MovieBaseController
{
    private readonly IAccountService _userService;
    private readonly IUserRatingService _userRatingService;
    private readonly ITitleService _titleService;
    private readonly IMapper _mapper;

    public UserRatingController(
        IAccountService userService,
        IUserRatingService userRatingService,
        ITitleService titleService,
        IMapper mapper,
        LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _userService = userService;
        _userRatingService = userRatingService;
        _titleService = titleService;
        _mapper = mapper;
    }

    // Get all title ratings from user
    [HttpGet("{username}/titlerating", Name = nameof(GetUserTitleRatings))]
    public IActionResult GetUserTitleRatings(string username, int page = 0, int pageSize = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        var (ratings, total) = _userRatingService.GetUserTitleRatings(username, page, pageSize);
        var items = ratings.Select(rating => new
        {
            Url = GetUrl("GetTitle", new { id = rating.TitleId }),
            PrimaryTitle = rating.Title.PrimaryTitle,
            Rating = rating.Rating,
            TimeStamp = rating.TimeStamp
        });
        return Ok(Paging(items, total, page, pageSize, nameof(GetUserTitleRatings), new RouteValueDictionary { { "username", username } }));
    }

    // Get rating for specific title from user
    [HttpGet("{username}/titlerating/{titleId}")]
    public IActionResult GetUserTitleRatingById(string username, string titleId, int page = 0, int pageSize = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(titleId, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        var userRating = _userRatingService.GetUserTitleRatings(username, page, pageSize)
            .Ratings
            .FirstOrDefault(ur => ur.TitleId == titleId.ToLower());

        if (userRating == null)
        {
            return NotFound();
        }

        return Ok(userRating);
    }


    [HttpPost("{username}/titlerating")]
    [Authorize]
    public IActionResult Post(
        string username,
        [FromBody] CreateTitleRatingModel userRating)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }

        if (userRating.Rating < 1 || userRating.Rating > 10)
        {
            return BadRequest("Rating must be between 1 and 10");
        }

        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(userRating.TitleId, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        var rating = ToUserTitleRatingDTO(username, userRating);

        if (!_userRatingService.CreateUserTitleRating(rating))
        {
            return StatusCode(500);
        }
        return CreatedAtAction(nameof(GetUserTitleRatingById), new { username, titleId = userRating.TitleId }, userRating);
    }

    [HttpDelete("{username}/titlerating/{titleId}")]
    [Authorize]
    public IActionResult Delete(string username, string titleId)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }

        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(titleId, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        if (!_userRatingService.UserTitleRatingExists(username, titleId, out var foundRating))
        {
            return NotFound();
        }

        if (!_userRatingService.DeleteUserTitleRating(foundRating))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    private UserTitleRatingDTO ToUserTitleRatingDTO(
        string username,
        CreateTitleRatingModel form)
    {
        return new UserTitleRatingDTO
        {
            Username = username,
            TitleId = form.TitleId,
            Rating = form.Rating,
            TimeStamp = DateTime.Now
        };
    }
}