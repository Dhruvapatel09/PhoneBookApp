using ApiApplicationCore.Controllers;
using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Contract;
using ApiApplicationCore.Services.Implementation;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContactApplicationCoreUnitTests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };
            // Act
            var result = target.RegisterUserService(registerDto);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be apphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "pass"
            };
            // Act
            var result = target.RegisterUserService(registerDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal(stringBuilder.ToString(), result.Message);
        }
        [Fact]
        public void RegisterUserService_ReturnsUserExists()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };
            // Act
            var result = target.RegisterUserService(registerDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists.", result.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsSomeThingWentWrong_WhenInValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };
            // Act
            var result = target.RegisterUserService(registerDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometime.", result.Message);
            mockAuthRepository.Verify(c => c.UserExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.LoginUserService(null);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometime.", result.Message);

        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns<User>(null);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.LoginUserService(loginDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(false);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.LoginUserService(loginDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockConfiguration.Setup(repo => repo.CreateToken(user)).Returns("");
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.LoginUserService(loginDto);
            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockConfiguration.Verify(repo => repo.CreateToken(user), Times.Once);
        }
        [Fact]
        public void ForgetPasswordService_ReturnsSomethingWentWrong_WhenForgetDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            // Act
            var result = target.ForgetPasswordService(null);
            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try again later.", result.Message);

        }

        [Fact]
        public void ForgetPasswordService_ReturnsInvalidUsername_WhenUserIsNull()
        {
            //Arrange
            var forgetDto = new ForegetDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns<User>(null);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            // Act
            var result = target.ForgetPasswordService(forgetDto);
            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Invalid username!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
        }
        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be apphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var forgetDto = new ForegetDto
            {
                UserName = "username",
                Password = "pass"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);
            // Act
            var result = target.ForgetPasswordService(forgetDto);
            // Assert
            Assert.False(result.Success);
            Assert.Equal(stringBuilder.ToString(), result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
        }
        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordsDontMatch()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForegetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);
            // Act
            var result = target.ForgetPasswordService(forgetDto);
            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Password and confirmation password do not match!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
        }
        [Fact]
        public void ForgetPasswordService_ReturnsSuccess_WhenPasswordResetSuccessfully()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForegetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);
            mockAuthRepository.Setup(repo => repo.UpdateUser(user));
            // Act
            var result = target.ForgetPasswordService(forgetDto);
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Password reset successfully!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
            mockAuthRepository.Verify(repo => repo.UpdateUser(user), Times.Once);
        }
        //GetUserDetailById

        [Fact]
        public void GetContactsById_ReturnEmpty_WhenNoContactExist()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<User>>
            {
                Success = false,

            };
 
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            //Act
            var actual = target.GetUser("loginId");

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found!", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        public void GetContactsById_ReturnsContact_WhenContactsExist()
        {
            //Arrange
            var user = new User
            {
                userId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                LoginId = "loginid",
                Email = "email@gmail.com",
                ContactNumber = "4758498576",

            };

            var response = new ServiceResponse<IEnumerable<User>>
            {
                Success = true,

            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IPasswordService>(); mockAuthRepository.Setup(c => c.GetUser(user.LoginId)).Returns(user);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            //Act
            var actual = target.GetUser(user.LoginId);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockAuthRepository.Verify(c => c.GetUser(user.LoginId), Times.Once);



        
        }
        //getUserByid
        [Fact]

        public void GetUserDetailById_ReturnsOk()
        {

            var loginId = "loginId";
            var user = new GetUserDto
            {

                LoginId = loginId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<GetUserDto>
            {
                Success = true,
                Data = new GetUserDto
                {
                    LoginId = loginId,
                    FirstName = user.FirstName
                }
            };

            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUser(loginId)).Returns(response);

            //Act
            var actual = target.GetUserById(loginId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUser(loginId), Times.Once);
        }

        [Fact]

        public void GetUserDetailById_ReturnsNotFound()
        {

            var loginId = "loginId";
            var user = new GetUserDto
            {

                LoginId = loginId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<GetUserDto>
            {
                Success = false,
                Data = new GetUserDto
                {
                    LoginId = loginId,
                    FirstName = user.FirstName
                }
            };

            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUser(loginId)).Returns(response);

            //Act
            var actual = target.GetUserById(loginId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUser(loginId), Times.Once);
        }

    }
}
