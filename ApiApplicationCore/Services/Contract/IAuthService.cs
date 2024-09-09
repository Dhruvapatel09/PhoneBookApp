using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;

namespace ApiApplicationCore.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> RegisterUserService(RegisterDto register);
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ForgetPasswordService(ForegetDto forgetDto);
        ServiceResponse<GetUserDto> GetUser(string id);
        ServiceResponse<string> ModifyUser(User user);
    }
}
