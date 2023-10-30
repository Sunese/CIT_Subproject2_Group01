using System.Security.Claims;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// This controller is solely for testing purposes and should be removed at some point
/// </summary>

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IFrameworkService _frameworkService;
    private readonly IJwtProvider _jwtProvider;

    public AuthController(
        IFrameworkService frameworkService,
        IJwtProvider jwtProvider)
    {
        _frameworkService = frameworkService;
        _jwtProvider = jwtProvider;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok("Authorized");
    }

    [HttpGet(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return Ok("Authorized admin");
    }

    [HttpGet(nameof(Anon))]
    [AllowAnonymous]
    public IActionResult Anon()
    {
        return Ok("Anonymous");
    }
}