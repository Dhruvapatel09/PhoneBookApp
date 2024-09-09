using PhoneBookApp.ViewModel;

namespace PhoneBookApp.Services.Contract
{
    public interface IAuthService
    {
        string RegisterUserService(RegisterViewModel register);

        string LoginUserService(LoginViewModel login);
    }
}
