using Application.Models;
using Domain.Models;

namespace API.Security;

public interface IJwtProvider
{
    string GenerateJwtToken(UserDTO user);
}

