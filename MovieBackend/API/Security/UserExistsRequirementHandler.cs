using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace API.Security;

public class UserExistsRequirementHandler : AuthorizationHandler<UserExistsRequirement>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<UserExistsRequirementHandler> _logger;
    
    public UserExistsRequirementHandler(
        IAccountService accountService,
        ILogger<UserExistsRequirementHandler> logger)
    {
        _logger = logger;
        _accountService = accountService;
    }
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserExistsRequirement requirement)
    {
        if (_accountService.UserExists(context.User.Identity!.Name!, out var foundUser))
        {
            _logger.LogInformation($"User '{foundUser.UserName}' exists!");
            context.Succeed(requirement);
        }
        else 
        {
            _logger.LogError($"Authenticated user: '{context.User.Identity.Name}' does not exist!");
        }
        return Task.CompletedTask;
    }
}