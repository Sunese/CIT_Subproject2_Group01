using System.Security.Claims;
using System.Text.RegularExpressions;
using API.Models;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.Framework;

[ApiController]
[Route("api/v1/account")]
public class AccountController : MovieBaseController
{
    private readonly IAccountService _accountService;
    private readonly IHashingService _hashingService;
    private readonly IJwtProvider _jwtProvider;

    public AccountController(
        IAccountService accountService,
        IHashingService hashingService,
        IJwtProvider jwtProvider,
        LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _accountService = accountService;
        _jwtProvider = jwtProvider;
        _hashingService = hashingService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginModel loginModel)
    {
        if (!IsValidUsername(loginModel.UserName))
        {
            return BadRequest();
        }
        if (!_accountService.UserExists(loginModel.UserName, out var storedUser))
        {
            return Unauthorized();
        }
        if (!_hashingService.Verify(loginModel.Password, storedUser.Password, storedUser.Salt))
        {
            return Unauthorized();
        }
        var jwtToken = "Bearer " + _jwtProvider.GenerateJwtToken(storedUser);
        return Ok(new { storedUser.UserName, token = jwtToken });
    }

    [HttpPost("register")]
    public IActionResult RegisterUser(RegisterModel registerModel)
    {
        if (!IsValidUsername(registerModel.UserName))
        {
            return BadRequest();
        }
        if (IsValidPassword(registerModel.Password))
        {
            return BadRequest();
        }
        if (IsValidEmail(registerModel.Email))
        {
            return BadRequest();
        } 

        var (hash, salt) = _hashingService.Hash(registerModel.Password);
        var newUser = new UserDTO
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email,
            Role = registerModel.Role,
            Password = hash,
            Salt = salt
        };
        _accountService.CreateUser(newUser);
        return Ok();
    }

    [HttpGet("{username}")]
    [Authorize]
    public IActionResult GetAccountInfo(string username)
    {
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (!_accountService.UserExists(username, out var storedUser))
        {
            return Unauthorized();
        }
        return Ok(new
        {
            username = storedUser.UserName,
            email = storedUser.Email,
            role = storedUser.Role,
        });

    }

    [HttpDelete("{username}")]
    [Authorize]
    public IActionResult Delete(string username)
    {
        if (!IsValidUsername(username))
        {
            return BadRequest();
        }
        if (!_accountService.UserExists(username, out _))
        {
            return Unauthorized();
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        _accountService.DeleteUser(username);
        return Ok();
    }

    [HttpPatch("{username}/password")]
    [Authorize]
    // TODO: the model does not actually adhere to the PATCH standard
    // i.e. {"op": "replace", "path": "/password", "value": "newPassword"}

    // UpdatePasswordDTO model from body because we don't want to expose the
    // password in the URL and when using HTTPS the body is encrypted
    public IActionResult UpdatePassword(string username, [FromBody] UpdatePasswordDTO model)
    {
        if (!IsValidPassword(model.NewPassword))
        {
            return BadRequest();
        }
        if (!_accountService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        var (hash, salt) = _hashingService.Hash(model.NewPassword);
        _accountService.UpdatePassword(username, hash, salt);
        return Ok();
    }

    [HttpPatch("{username}/email")]
    [Authorize]
    public IActionResult UpdateEmail(string username, [FromBody] UpdateEmailDTO model)
    {
        if (!IsValidEmail(model.NewEmail))
        {
            return BadRequest();
        }
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