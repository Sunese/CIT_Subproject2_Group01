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

    public UserController(ILogger<UserController> logger, IFrameworkService frameworkService)
    {
        _logger = logger;
        _frameworkService = frameworkService;
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
    public IActionResult Post([FromBody] UserDTO user)
    {
        if (_frameworkService.GetUser(user.UserName) is not null)
        {
            return BadRequest("User with specified username already exists");
        }
        else if (_frameworkService.AddUser(user))
        {
            return Ok();
        }
        else
        {
            return BadRequest("User could not be added");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (_frameworkService.UserExists(id, out UserDTO user))
        {
            _frameworkService.DeleteUser(user);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}