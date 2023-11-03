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
public class AccountController : FrameworkBaseController
{
    private readonly IAccountService _accountService;
    private readonly IHashingService _hashingService;
    private readonly IJwtProvider _jwtProvider;

    public AccountController(
        IAccountService accountService,
        IHashingService hashingService,
        IJwtProvider jwtProvider)
    {
        _accountService = accountService;
        _jwtProvider = jwtProvider;
        _hashingService = hashingService;
    }

    [HttpPost("login")]
    public IActionResult Login(UserDTO loginUser)
    {
        if (!_accountService.UserExists(loginUser.UserName, out var storedUser))
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
        if (_accountService.UserExists(user.UserName, out _))
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

        _accountService.CreateUser(user);
        return Ok();
    }

    [HttpDelete("{username}")]
    [Authorize]
    public IActionResult Delete(string? username)
    {
        if (!_accountService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _accountService.DeleteUser(username);
        return Ok();
    }

    [HttpPatch("{username}/password", Name = nameof(UpdatePassword))]
    [Authorize]
    // UpdatePasswordDTO model from body because we don't want to expose the 
    // password in the URL and when using HTTPS the body is encrypted
    public IActionResult UpdatePassword(string username, [FromBody] UpdatePasswordDTO model)
    {
        if (!_accountService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _accountService.UpdatePassword(username, model.NewPassword);
        return Ok();
    }

    [HttpPatch("{username}/email", Name = nameof(UpdateEmail))]
    [Authorize]
    public IActionResult UpdateEmail(string username, [FromBody] UpdateEmailDTO model)
    {
        if (!_accountService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _accountService.UpdateEmail(username, model.NewEmail);
        return Ok();
    }
}