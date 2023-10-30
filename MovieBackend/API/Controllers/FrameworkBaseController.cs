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
    protected bool IsAuthorizedToUpdate(string username)
    {
        if (HttpContext.User.Identity is null)
        {
            return false;
        }
        var authorizedUsername = HttpContext.User.Identity.Name;
        var isAdmin = HttpContext.User.IsInRole("Admin");

        if (isAdmin) // always allow admins to make update
        {
            return true;
        }
        else if (authorizedUsername.IsNullOrEmpty())
        {
            return false;
        }
        else if (authorizedUsername.ToLower() != username.ToLower())
        {
            return false;
        }

        return true;
    }
}