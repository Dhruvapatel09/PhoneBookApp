using ApiApplicationCore.Models;

namespace ApiApplicationCore.Services.Contract
{
    public interface IPasswordService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
