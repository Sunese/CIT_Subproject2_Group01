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
public class UserController : FrameworkBaseController
{
    private readonly IUserService _frameworkService;
    private readonly IHashingService _hashingService;
    private readonly IJwtProvider _jwtProvider;

    public UserController(
        IUserService frameworkService,
        IHashingService hashingService,
        IJwtProvider jwtProvider)
    {
        _frameworkService = frameworkService;
        _jwtProvider = jwtProvider;
        _hashingService = hashingService;
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
        var jwtToken = "Bearer " + _jwtProvider.GenerateJwtToken(storedUser);
        return Ok(new { storedUser.UserName, token = jwtToken });
    }

    [HttpPost("register")]
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

        _frameworkService.CreateUser(user);
        return Ok();
    }

    [HttpDelete("{username}")]
    [Authorize]
    public IActionResult Delete(string? username)
    {
        if (!_frameworkService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _frameworkService.DeleteUser(username);
        return Ok();
    }

    [HttpPatch("{username}/password", Name = nameof(UpdatePassword))]
    [Authorize]
    // UpdatePasswordDTO model from body because we don't want to expose the 
    // password in the URL and when using HTTPS the body is encrypted
    public IActionResult UpdatePassword(string username, [FromBody] UpdatePasswordDTO model)
    {
        if (!_frameworkService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _frameworkService.UpdatePassword(username, model.NewPassword);
        return Ok();
    }

    [HttpPatch("{username}/email", Name = nameof(UpdateEmail))]
    [Authorize]
    public IActionResult UpdateEmail(string username, [FromBody] UpdateEmailDTO model)
    {
        if (!_frameworkService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _frameworkService.UpdateEmail(username, model.NewEmail);
        return Ok();
    }
}