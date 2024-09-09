using ClientApplicationCore.Controllers;
using ClientApplicationCore.Infrastructure;
using ClientApplicationCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplicationCoreTests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public void Register_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.Register() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        //Register Post
        [Fact]
        public void Register_ModelIsInvalid()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel { FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("LastName", "last name is required");
            //Act
            var actual = target.Register(registerViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(registerViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_RedirectToRegisterSuccess_WhenUserSavedSuccessfully()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            { FirstName = "firstname", LastName = "lastname", Password = "Password@123", Email = "email@gmail.com", ConfirmPassword = "Password@123", ContactNumber = "1234567890", LoginId = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "User saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("RegisterSuccess", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_ReturnView_WhenBadRequest()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            { FirstName = "firstname", LastName = "lastname", Password = "Password@123", Email = "email@gmail.com", ConfirmPassword = "Password@123", ContactNumber = "1234567890", LoginId = "loginid" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_ReturnView_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            { FirstName = "firstname", LastName = "lastname", Password = "Password@123", Email = "email@gmail.com", ConfirmPassword = "Password@123", ContactNumber = "1234567890", LoginId = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try after sometime", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        //HttpGet RegisterSuccess
        [Fact]
        public void RegisterSuccess_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.RegisterSuccess() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //HttpGet Login
        [Fact]
        public void Login_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //Login HttpPost
        [Fact]
        public void Login_ModelIsInvalid()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "Username is required");
            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void Login_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Login_Success_RedirectToAction()
        {
            //Arrange
            var loginViewModel = new LoginViewModel { UserName = "loginid", Password = "Password" };
            var mockToken = "mockToken";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,

            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
             .Returns(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Once);
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Once);
            Assert.True(target.ModelState.IsValid);

        }
        [Fact]
        public void Login_RedirectToAction_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try after sometime", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        [Fact]
        public void Logout_DeleteJwtToken()
        {
            //Arrange
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Delete("jwtToken"));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.Logout() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockResponseCookie.Verify(c => c.Delete("jwtToken"), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Once);
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Once);
        }
        [Fact]
        public void ForgotPassword_ReturnView()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);
            var id = 1;
            //Act
            var actual = target.ForgotPassword() as ViewResult;

            //Assert
            Assert.NotNull(actual);

        }
        [Fact]
        public void ForgotPasswordConfirmation_ReturnView()
        {
            //Arrange

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object);
            var id = 1;
            //Act
            var actual = target.ForgotPasswordConfirmation() as ViewResult;

            //Assert
            Assert.NotNull(actual);

        }
        [Fact]
        public async Task ForgotPassword_ModelIsInvalid()
        {
            // Arrange
            var viewModel = new ForegetViewModel
            {
                Password = "Password@123"
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "Username is required");
            //Act
            var actual =await target.ForgotPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

            Assert.False(target.ModelState.IsValid);
        }



        [Fact]
        public async Task ForgotPasswordConfirmation_ReturnView_WhenBadRequest()
        {
            // Arrange
            var viewModel = new ForegetViewModel
            { UserName = "loginid", Password = "Password@123", ConfirmPassword = "Password@123" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);


            mockHttpClientService.Setup(c => c.PostHttpResponseMessageAsync(It.IsAny<string>(), viewModel, mockHttpRequest.Object))
               .ReturnsAsync(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = await target.ForgotPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessageAsync(It.IsAny<string>(), viewModel, mockHttpRequest.Object), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }



        [Fact]
        public async Task ForgotPasswordConfirmation_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var viewModel = new ForegetViewModel
            { UserName = "loginid", Password = "Password@123", ConfirmPassword = "Password@123" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);



            mockHttpClientService.Setup(c => c.PostHttpResponseMessageAsync(It.IsAny<string>(), viewModel, mockHttpRequest.Object))
              .ReturnsAsync(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = await target.ForgotPassword(viewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessageAsync(It.IsAny<string>(), viewModel, mockHttpRequest.Object), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        [Fact]
        public async Task ForgotPasswordConfirmation_Success_RedirectToAction()
        {
            // Arrange
            var viewModel = new ForegetViewModel
            { UserName = "loginid", Password = "Password@123", ConfirmPassword = "Password@123" };


            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,

            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };


            mockHttpClientService.Setup(c => c.PostHttpResponseMessageAsync(It.IsAny<string>(), viewModel, mockHttpRequest.Object))
              .ReturnsAsync(expectedResponse);

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = await target.ForgotPassword(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPasswordConfirmation", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.True(target.ModelState.IsValid);

        }

    }
}
