using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface IAccountService
{
    bool UserExists(string username, out UserDTO user);
    void CreateUser(UserDTO user);
    void DeleteUser(string username);
    void UpdatePassword(string username, string newPassword, string newSalt);
    void UpdateEmail(string username, string newEmail);
}