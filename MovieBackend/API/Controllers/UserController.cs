using System.Security.Claims;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IFrameworkService _frameworkService;
    private readonly IHashingService _hashingService;
    private readonly IJwtProvider _jwtProvider;

    public UserController(
        ILogger<UserController> logger, 
        IFrameworkService frameworkService,
        IHashingService hashingService,
        IJwtProvider jwtProvider)
    {
        _logger = logger;
        _frameworkService = frameworkService;
        _jwtProvider = jwtProvider;
        _hashingService = hashingService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_frameworkService.GetUsers());
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        if (_frameworkService.GetUser(id) is UserDTO user)
        {
            return Ok(user);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult RegisterUser(UserDTO user)
    {
        if (_frameworkService.UserExists(user.UserName, out _))
        {
            return BadRequest("User with specified username already exists");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Password cannot be empty");
        }

        var (hash, salt) = _hashingService.Hash(user.Password);
        user.Password = hash;
        user.Salt = salt;

        if (!_frameworkService.CreateUser(user))
        {
            return StatusCode(500, "Failed to create user");
        }

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(UserDTO loginUser)
    {
        if (!_frameworkService.UserExists(loginUser.UserName, out var storedUser))
        {
            return BadRequest("User does not exist");
        }
        if (!_hashingService.Verify(loginUser.Password, storedUser.Password, storedUser.Salt))
        {
            return BadRequest("Incorrect password");
        }
        var jwtToken = _jwtProvider.GenerateJwtToken(storedUser);
        return Ok(new { storedUser.UserName, token = jwtToken });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(UserDTO user)
    {
        if (!_frameworkService.UserExists(user.UserName, out var storedUser))
        {
            return BadRequest("User does not exist");
        }
        if (!_hashingService.Verify(user.Password, storedUser.Password, storedUser.Salt))
        {
            return BadRequest("Incorrect password");
        }
        if (!_frameworkService.DeleteUser(user))
        {
            return StatusCode(500, "Failed to delete user");
        }
        return Ok($"{user.UserName} deleted");
    }
}