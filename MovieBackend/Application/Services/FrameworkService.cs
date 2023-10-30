using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Services;

public class FrameworkService : IFrameworkService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public FrameworkService(ImdbContext imdbContext, IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }

    public bool AddUser(UserDTO user)
    {
        _imdbContext.Users.Add(_mapper.Map<UserDTO, User>(user));
        return _imdbContext.SaveChanges() == 1;
    }

    public bool UserExists(string username, out UserDTO userDTO)
    {
        var user = _imdbContext.Users.FirstOrDefault(u => u.UserName == username);
        // Detach the user from the context to avoid tracking
        _imdbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        userDTO = _mapper.Map<User, UserDTO>(user);
        return user is not null;
    }

    public bool DeleteUser(UserDTO userDTO)
    {
        var user = _mapper.Map<UserDTO, User>(userDTO);
        _imdbContext.Users.Remove(user);
        return _imdbContext.SaveChanges() == 1;
    }

    public UserDTO? GetUser(string username)
    {
        var user = _imdbContext.Users.FirstOrDefault(u => u.UserName == username);
        return user is null ? null : _mapper.Map<User, UserDTO>(user);
    }

    public IList<UserDTO> GetUsers()
    {
        var users = _imdbContext.Users.ToList();
        return _mapper.Map<List<User>, List<UserDTO>>(users);
    }
}