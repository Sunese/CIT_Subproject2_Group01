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
            Url = GetUrl("GetTitle", new { id = rating.TitleID }),
            Title = rating.Title,
            TitleID = rating.TitleID,
            Rating = rating.Rating,
            TimeStamp = rating.TimeStamp
        });
        return Ok(Paging(items, total, page, pageSize, nameof(GetUserTitleRatings), new RouteValueDictionary { { "username", username } }));
    }

    // Get rating for specific title from user
    [HttpGet("{username}/titlerating/{titleID}")]
    public IActionResult GetUserTitleRatingById(string username, string titleID, int page = 0, int pageSize = 10)
    {
        if (!IsValidTitleID(titleID))
        {
            return BadRequest();
        }
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(titleID, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        if (!_userRatingService.TryGetUserTitleRating(username, titleID, out var foundRating))
        {
            return NotFound();
        }

        return Ok(foundRating);
    }


    [HttpPost("{username}/titlerating")]
    [Authorize]
    public IActionResult Post(
        string username,
        [FromBody] CreateTitleRatingModel userRating)
    {
        if (!IsValidTitleID(userRating.TitleID))
        {
            return BadRequest();
        }
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

        if (!_titleService.TitleExists(userRating.TitleID, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        var rating = ToUserTitleRatingDTO(username, userRating);

        if (!_userRatingService.CreateUserTitleRating(rating))
        {
            return StatusCode(500);
        }
        return CreatedAtAction(nameof(GetUserTitleRatingById), new { username, titleID = userRating.TitleID }, userRating);
    }

    [HttpDelete("{username}/titlerating/{titleID}")]
    [Authorize]
    public IActionResult Delete(string username, string titleID)
    {
        if (!IsValidTitleID(titleID))
        {
            return BadRequest();
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }

        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(titleID, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        if (!_userRatingService.UserTitleRatingExists(username, titleID, out var foundRating))
        {
            return NotFound();
        }

        if (!_userRatingService.DeleteUserTitleRating(foundRating))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    [HttpPut("{username}/titlerating/{titleID}")]
    [Authorize]
    public IActionResult Update(string username, string titleID, [FromBody] CreateTitleRatingModel newRatingModel)
    {
        if (!IsValidTitleID(titleID))
        {
            return BadRequest();
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }

        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        if (!_titleService.TitleExists(titleID, out _))
        {
            return BadRequest("TitleName does not exist");
        }

        if (!_userRatingService.UserTitleRatingExists(username, titleID, out var foundRating))
        {
            return BadRequest();
        }

        var newRating = ToUserTitleRatingDTO(username, newRatingModel);

        if (!_userRatingService.ReplaceUserTitleRating(foundRating, newRating))
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
            TitleID = form.TitleID,
            Rating = form.Rating,
            TimeStamp = DateTime.Now
        };
    }
}