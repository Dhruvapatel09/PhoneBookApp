using ApiApplicationCore.Models;

namespace ApiApplicationCore.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);

        User? ValidateUser(string username);

        bool UserExists(string loginId, string email);

        bool UpdateUser(User user);
        User? GetUser(string id);
        bool UserExist(int userId, string loginId, string email);
    }
}
