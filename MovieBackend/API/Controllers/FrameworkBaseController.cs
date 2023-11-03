using System.Security.Claims;
using API.Security;
using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class FrameworkBaseController : ControllerBase
{
    protected bool OwnsResource(string username)
    {
        if (HttpContext.User.Identity is null)
        {
            return false;
        }
        else if (HttpContext.User.Identity.Name.IsNullOrEmpty())
        {
            return false;
        }

        var authenticatedUsername = HttpContext.User.Identity.Name.ToLower();
        if (authenticatedUsername.ToLower() != username.ToLower())
        {
            return false;
        }

        return true;
    }

    protected bool IsSignedIn()
    {
        return HttpContext.User.Identity.IsAuthenticated;
    }

    protected bool IsAdmin()
    {
        return HttpContext.User.IsInRole("Admin");
    }
}