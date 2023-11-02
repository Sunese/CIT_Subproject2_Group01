using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("")]
public class UserRatingController : FrameworkBaseController
{
    private readonly IUserService _userService;
    private readonly IUserRatingService _userRatingService;

    public UserRatingController(
        IUserService userService,
        IUserRatingService userRatingService)
    {
        _userService = userService;
        _userRatingService = userRatingService;
    }

    [HttpGet("{username}/rating")]
    public IActionResult Get(string username, int count = 10)
    {
        if (!_userService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }

        return Ok(_userRatingService.GetUserTitleRatings(username, count));
    }
}