using System.Security.Cryptography;
using System.Text;

namespace API.Security;

public interface IHashingService
{
    (string hash, string salt) Hash(string password);
    bool Verify(string loginPassword, string hashedRegisteredPassword, string saltString);
}