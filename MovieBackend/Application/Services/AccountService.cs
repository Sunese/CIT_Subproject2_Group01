using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public AccountService(
        ImdbContext imdbContext, 
        IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }

    public void CreateUser(UserDTO user)
    {
        var newUser = _mapper.Map<UserDTO, User>(user);
        FormattableString query = $"call adduser({newUser.UserName}, {newUser.Password}, {newUser.Salt}, {newUser.Email}, {newUser.Role})";
        _imdbContext.Database.ExecuteSqlInterpolated(query);
    }

    public bool UserExists(string username, out UserDTO userDTO)
    {
        var user = _imdbContext.Users.Find(username);
        if (user is null)
        {
            userDTO = null;
            return false;
        }
        // Detach the user from the context to avoid tracking
        _imdbContext.Entry(user).State = EntityState.Detached;
        userDTO = _mapper.Map<User, UserDTO>(user);
        return user is not null;
    }

    public void DeleteUser(string username)
    {
        FormattableString query = $"call deleteuser({username})";
        _imdbContext.Database.ExecuteSqlInterpolated(query);
    }

    public void UpdatePassword(string username, string newPassword, string newSalt)
    {
        FormattableString query = $"call updateuserpassword({username}, {newPassword}, {newSalt})";
        _imdbContext.Database.ExecuteSqlInterpolated(query);
    }

    public void UpdateEmail(string username, string newEmail)
    {
        FormattableString query = $"call updateuseremail({username}, {newEmail})";
        _imdbContext.Database.ExecuteSqlInterpolated(query);
    }
}