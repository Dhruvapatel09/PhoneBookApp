using ApiApplicationCore.Controllers;
using ApiApplicationCore.Dto;
using ApiApplicationCore.Services.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiContactApplicationCoreUnitTests.Controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.Register(registerDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }
        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.Login(loginDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                LoginId = "loginid",
                Email = "email@email.com",
                Password = "password",
                ConfirmPassword = "password",
                ContactNumber = "1234567890",
                FirstName = "firstname",
                LastName = "lastname"
            };
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty
            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);
            var controller = new AuthController(mockAuthService.Object);
            // Act
            var actual = controller.Register(registerDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }
        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { UserName = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.Login(loginDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Theory]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [InlineData("Invalid username!")]
        [InlineData("Password and confirmation password do not match!")]
        [InlineData("Something went wrong, please try again later.")]
        public void ForgetPassword_ReturnsBadRequest_WhenForgetPasswordFails(string message)
        {
            // Arrange
            var forgetDto = new ForegetDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.ForgetPassword(forgetDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }
        [Fact]
        public void ForgetPassword_ReturnsOk_WhenForgetPasswordSucceeds()
        {
            // Arrange
            var fixture = new Fixture();
            var forgetDto = new ForegetDto()
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = ""
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.ForgetPassword(forgetDto) as OkObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }
    }
}
