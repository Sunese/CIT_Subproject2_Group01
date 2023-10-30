using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface IFrameworkService
{
    IList<UserDTO>? GetUsers();
    UserDTO? GetUser(string username);
    bool UserExists(string username, out UserDTO user);
    bool CreateUser(UserDTO user);
    bool DeleteUser(UserDTO user);
}