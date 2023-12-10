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
        if (isInvalidUsername(registerModel.UserName))
        {
            return BadRequest();
        }
        if (isInvalidPassword(registerModel.Password))
        {
            return BadRequest();
        }
        if (isInvalidEmail(registerModel.Email))
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
        if (!_accountService.UserExists(username, out _))
        {
            return BadRequest("User does not exist");
        }
        if (!OwnsResource(username))
        {
            return Unauthorized();
        }
        if (isInvalidPassword(model.NewPassword))
        {
            return BadRequest();
        }

        var (hash, salt) = _hashingService.Hash(model.NewPassword);
        _accountService.UpdatePassword(username, hash, salt);
        return Ok();
    }

    [HttpPatch("{username}/email")]
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
        if (isInvalidEmail(model.NewEmail))
        {
            return BadRequest();
        }
        _accountService.UpdateEmail(username, model.NewEmail);
        return Ok();
    }

    private bool isInvalidUsername(string username) 
    {
        if (_accountService.UserExists(username, out _))
        {
            return true;
        }
        if (username.Length < 3)
        {
            return true;
        }
        if (!Regex.IsMatch(username, "^[a-z0-9._]*$"))
        {
            return true;
        }
        return false;
    }

    private bool isInvalidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return true;
        }
        if (password.Length < 8)
        {
            return true;
        }
        return false;
    }

    private bool isInvalidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return true;
        }
        if (!Regex.IsMatch(email, "^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$"))
        {
            return true;
        }
        return false;
    }
}