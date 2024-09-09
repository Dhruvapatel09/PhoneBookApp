using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Contract;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiApplicationCore.Services.Implementation
{
    public class AuthService : IAuthService

    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;
        public AuthService(IAuthRepository authRepository, IPasswordService passwordService)
        {
            _authRepository = authRepository;
            _passwordService = passwordService;
        }

        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }
                else
                {
                    // Save user
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        ContactNumber = register.ContactNumber,
                        FileName = register.FileName,
                        File = register.File,
                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wrong, please try after sometime.";
                }
            }
            return response;

        }

        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();

            if (login != null)
            {

                var user = _authRepository.ValidateUser(login.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password!";
                    return response;
                }
                else if (!_passwordService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;

                    response.Message = "Invalid username or password!";
                    return response;
                }

                string token = _passwordService.CreateToken(user);
                response.Data = token;
                response.Message = "Loged In Successfully!";
                return response;
            }
            response.Success = false;
            response.Message = "Something went wrong, please try after sometime.";

            return response;
        }
        [ExcludeFromCodeCoverage]
        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be apphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
        [ExcludeFromCodeCoverage]
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public ServiceResponse<string> ForgetPasswordService(ForegetDto forgetDto)
        {
            var response = new ServiceResponse<string>();

            if (forgetDto != null)
            {
                var user = _authRepository.ValidateUser(forgetDto.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username!";
                    return response;
                }

                var message = CheckPasswordStrength(forgetDto.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }

                if (forgetDto.Password != forgetDto.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Password and confirmation password do not match!";
                    return response;
                }

                // Create password hash and salt
                CreatePasswordHash(forgetDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Update user's password hash and salt
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _authRepository.UpdateUser(user); // Update the user with the new password hash and salt

                response.Success = true;
                response.Message = "Password reset successfully!";
                return response;
            }

            response.Success = false;
            response.Message = "Something went wrong, please try again later.";
            return response;
        }
        public ServiceResponse<GetUserDto> GetUser(string id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var existingUser = _authRepository.GetUser(id);
            if (existingUser != null)
            {
                var user = new GetUserDto()
                {
                    userId = existingUser.userId,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    ContactNumber = existingUser.ContactNumber,
                    File = existingUser.File,
                    Email = existingUser.Email,
                    FileName = existingUser.FileName,
                    LoginId = existingUser.LoginId,

                };
                response.Data = user;

            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;


        }
        public ServiceResponse<string> ModifyUser(User user)
        {
            var response = new ServiceResponse<string>();
            if (_authRepository.UserExist(user.userId, user.LoginId, user.Email))
            {
                response.Success = false;
                response.Message = "User Exists!";
                return response;

            }
            var existingContact = _authRepository.GetUser(user.LoginId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = user.FirstName;
                existingContact.LastName = user.LastName;
                existingContact.Email = user.Email;
                existingContact.File = user.File;
                existingContact.ContactNumber = user.ContactNumber;
                existingContact.FileName= user.FileName;
                result = _authRepository.UpdateUser(existingContact);
            }
            if (result)
            {
                response.Success = true;
                response.Message = "User Updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong after sometime";
            }
            return response;
        }
    }
}
