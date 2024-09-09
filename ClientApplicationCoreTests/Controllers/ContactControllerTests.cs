using AutoFixture;
using ClientApplicationCore.Controllers;
using ClientApplicationCore.Infrastructure;
using ClientApplicationCore.ViewModels;
using Microsoft.AspNetCore.Hosting;
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
    public class ContactControllerTests
    {
        [Fact]
        public void ShowAllContactWithPagination_ReturnsEmptyList_WhenLetterIsNotNull()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string? searchQuery = "f";
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);


        }
        [Fact]
        public void ShowAllContactWithPagination_ReturnsCorrectView_WhenSearchQueryIsNotEmpty()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { }; // Empty list as expected result

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<List<ContactViewModel>>(actual.Model);
            var model = actual.Model as List<ContactViewModel>;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void ShowAllContactWithPagination_ReturnsEmptyList_WhenLetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string? searchQuery = null;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);


        }
        [Fact]
        public void ShowAllContactWithPagination_ReturnsEmptyView_WhenTotalCountIsZero()
        {
            // Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            var model = actual.Model as List<ContactViewModel>;
            Assert.Empty(model);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPagination_ReturnsRedirectToIndex_WhenPageIsGreaterThanTotalPages()
        {
            // Arrange
            char? letter = 'A';
            int page = 4;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel>();
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ShowAllContactWithPagination", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
            Assert.Equal(pageSize, redirectToActionResult.RouteValues["pageSize"]);
            Assert.Equal(letter, redirectToActionResult.RouteValues["letter"]);
            Assert.Equal(searchQuery, redirectToActionResult.RouteValues["searchQuery"]);
            Assert.Equal(sortOrder, redirectToActionResult.RouteValues["sortOrder"]);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPagination_ReturnsContacts_WhenSerachAndLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ PhoneId=1, FirstName="FirstName 1"},
                new ContactViewModel{ PhoneId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 4 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ShowAllContactWithPagination('f',  1, 2, "fir", "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachAndLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ PhoneId=1, FirstName="FirstName 1"},
                new ContactViewModel{ PhoneId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ShowAllContactWithPagination('f', 1, 2, "fir", "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPagination()
        {
            // Arrange
            char? letter = 'A';
            int page = 4;
            int pageSize = 2;
            string searchQuery = "testSearchQuery";
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel>();
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery, sortOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ShowAllContactWithPagination", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
            Assert.Equal(pageSize, redirectToActionResult.RouteValues["pageSize"]);
            Assert.Equal(letter, redirectToActionResult.RouteValues["letter"]);
            Assert.Equal(searchQuery, redirectToActionResult.RouteValues["searchQuery"]);
            Assert.Equal(sortOrder, redirectToActionResult.RouteValues["sortOrder"]);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void Index_ReturnsEmptyCategories()
        {
            //Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string? searchQuery = "D";
            var expectedContacts = new List<ContactViewModel>
            {
            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>()
            {
                Success = true,
                Data = null
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockImage = new Mock<IImageUpload>();
            // Set up HttpRequest and HttpContext
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var apiGetContactsUrl = "fakeEndPointContact/GetAllContactsByPagination/?letter=A&page=1&pageSize=2";
            var apiGetCountUrl = "fakeEndPointContact/GetContactsCount/?letter=A";

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(apiGetContactsUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedResponse);

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = 0
            };

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(expectedCountResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPagination(letter, page, pageSize, searchQuery) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            var model = actual.Model as IEnumerable<ContactViewModel>;
            Assert.Equal(expectedContacts.Count, model.Count());

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

        }
        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var expectedStates = new List<StateViewModel>
    {
        new StateViewModel { StateId = 1, StateName = "State 1" },
        new StateViewModel { StateId = 2, StateName = "State 2" }
    };

            var expectedCountries = new List<CountryViewModel>
    {
        new CountryViewModel { CountryId = 1, CountryName = "Country 1" },
        new CountryViewModel { CountryId = 2, CountryName = "Country 2" }
    };

            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>("fakeEndPointState/GetStates", HttpMethod.Get, mockHttpRequest.Object, null, 60))
                  .Returns(stateResponse);

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>("fakeEndPointCountry/GetAll", HttpMethod.Get, mockHttpRequest.Object, null, 60))
                .Returns(countryResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>("fakeEndPointState/GetStates", HttpMethod.Get, mockHttpRequest.Object, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>("fakeEndPointCountry/GetAll", HttpMethod.Get, mockHttpRequest.Object, null, 60), Times.Once);

        }
        [Fact]
        public void Create_ReturnsView_WithNullData()
        {

            //Arrange
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockImage = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>(); var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResultWithErrorMessage_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel {  FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);

            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResultWithErrorMessage_WhenErrorResponseIsNull()
        { 
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedErrorResponse = new ServiceResponse<string>
            {  };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };

            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResult_WithContactList_WhenModelStateIsInvalid()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var expectedResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedResponseState = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var viewModel = new AddContactViewModel { FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponseState);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResult_WithEmptyCountryandStateList_WhenModelStateIsInvalid()
        {
            //Arrange

            var expectedResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false,
            };
            var expectedResponseState = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,
            };
            var viewModel = new AddContactViewModel { FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseState);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully_WhenFileISNotNull()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},

            };

            var viewModel = new AddContactViewModel {FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry, file = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new UpdateContactViewModel { PhoneId = 1, FirstName = "Category 1" };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedCountries = new List<CountryViewModel>
                {
                    new CountryViewModel{},

                };
            var expectedStates = new List<StateViewModel>
                {
                    new StateViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };

            var mockImage = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;

            //Assert
            var model = actual.Model as UpdateContactViewModel;
            Assert.NotNull(model);
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void Edit_ReturnsErrorMessage_WhenStatusCodeIsSuccess()
        {
            // Arrange
            var id = 1;

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedCountries = new List<CountryViewModel>
        {
            new CountryViewModel{},
        };
            var expectedStates = new List<StateViewModel>
        {
            new StateViewModel{},
        };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();

            var mockImage = new Mock<IImageUpload>();
            var errorMessage = "Failed to update";
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Data = null,
                Success = false,
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void Edit_ReturnsErrorMessage_WhenStatusCodeIsNotSuccess()
        {
            // Arrange
            var id = 1;

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedCountries = new List<CountryViewModel>
        {
            new CountryViewModel{},
        };
            var expectedStates = new List<StateViewModel>
        {
            new StateViewModel{},
        };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();


            var errorMessage = "Failed to update";
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Data = null,
                Success = false,
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void Edit_ReturnsErrorMessage_WhenErrorResponseIsNull()
        {
            // Arrange
            var id = 1;

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedCountries = new List<CountryViewModel>
        {
            new CountryViewModel{},
        };
            var expectedStates = new List<StateViewModel>
        {
            new StateViewModel{},
        };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();



            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal("Something went wrong please try after some time.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
         [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var viewModel = new UpdateContactViewModel { PhoneId = id, FirstName = "F1", LastName = "L1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var successMessage = "Product saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction_RemoveImage()
        {
            //Arrange
            var id = 1;
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var viewModel = new UpdateContactViewModel { PhoneId = id, FirstName = "C1", file = new FormFile(new MemoryStream(new byte[1]), 0, 0, "xyz", "xyz.jpg"), Image = "xyz.jpg", RemoveImage = true };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse); mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Empty(viewModel.Image);
            Assert.Null(viewModel.ImageByte);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ContactSavedSuccessfully_RedirectToAction_WithFile()
        {
            //Arrange
            var id = 1;
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var viewModel = new UpdateContactViewModel { PhoneId = id, FirstName = "C1", file = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.pdf"), Image = "xyz.pdf" };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint"); 
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var errorMessage = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
            var mockHttpContext = new Mock<HttpContext>();
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        [Fact]
        public void Edit_ContactFailedToSaveServiceResponseNull_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var viewModel = new UpdateContactViewModel { PhoneId = id, FirstName = "C1" };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };

            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ContactFailed_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var viewModel = new UpdateContactViewModel()
            {
                PhoneId = 1,
                FirstName = "C1",

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            var countries = new List<CountryViewModel>
                {
                new CountryViewModel { CountryId =1, CountryName = "C1"},
                new CountryViewModel { CountryId =2, CountryName = "C2"},
             };

            var states = new List<StateViewModel>
                {
                new StateViewModel { CountryId =1, StateName = "C1", StateId = 1},
                new StateViewModel { CountryId =2, StateName = "C2", StateId = 2},
             };

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = countries
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = states
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ContactFailed_WhenModelStateIsInvalid_WhenStateCountryAreNull()
        {
            //Arrange
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var viewModel = new UpdateContactViewModel()
            {
                PhoneId = 1,
                FirstName = "C1",

            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var expectedResponseCountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false
            };
            var expectedResponseStates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseStates);
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.False(target.ModelState.IsValid);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Edit_ContactFailedToSave_ReturnRedirectToActionResult()
        {
            //Arrange
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var viewModel = new UpdateContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
           .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsSomethingWentWrong_ReturnRedirectToActionResult()
        {
            //Arrange
            var expectedCountries = new List<CountryViewModel>
            {
                new CountryViewModel{},

            };
            var expectedStates = new List<StateViewModel>
            {
                new StateViewModel{},

            };
            var stateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var viewModel = new UpdateContactViewModel { FirstName = "C1", LastName = "D1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Something went wrong. Please try after sometime.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
          .Returns(stateResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(countryResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);


        }
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNotNull()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new UpdateContactViewModel { PhoneId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry, file = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object,mockWebHostEnvironment.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        public void Details_WhenSuccessResponseIsOK_ReturnView_WhenDataIsReceived()
        {
            // Arrange
            var id = 1;
            var expectedContacts = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = new ContactViewModel { PhoneId = 2, FirstName = "Fname 2", LastName = "Lname 2", Email = "test@email.com", PhoneNumber = "0000000000", Gender = "f", Favourites = true, Company = "Company", Image = "Image.png", CountryId = 1, StateId = 1 }
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedContacts))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void Details_WhenSuccessResponseIsOK_WhenDataIsNullAndSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Data is not found";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Message = errorMessage,
                Data = null
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Details(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ShowAllContactWithPagination", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Details_ReturnsRedirectWithErrorMessage_WhenServiceResponseIsNotSuccessful()
        {
            // Arrange
            int phoneId = 1;
            var errorMessage = "An error occurred while processing the request.";

            var expectedErrorResponseContent = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Message = errorMessage
            };

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponseContent))
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedErrorResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = target.Details(phoneId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }


        [Fact]
        public void Details_ReturnsRedirectWithErrorMessage_WhenServiceResponseIsNull()
        {
            // Arrange
            int phoneId = 1;
            var errorMessage = "Something went wrong.Please try after sometime.";

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedErrorResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = target.Details(phoneId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }


        [Fact]
        public void Delete_ReturnViewWithContact_WhenContactExists()
        {
            // Arrange
            int phoneId = 1;
            var contactViewModel = new ContactViewModel
            {
                PhoneId = 1,
                FirstName = "Fname 1",
                LastName = "Lname 1"
            };

            var expectedSuccessResponseContent = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Message = string.Empty,
                Data = contactViewModel
            };

            var expectedSuccessResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedSuccessResponseContent))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedSuccessResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
            };

            // Act
            var actual = target.Delete(phoneId) as ViewResult;
            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contactViewModel.PhoneId, ((ContactViewModel)actual.Model).PhoneId);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsRedirectWithErrorMessage_WhenServiceResponseIsNotSuccessful()
        {
            // Arrange
            int phoneId = 1;
            var errorMessage = "An error occurred while processing the request.";

            var expectedErrorResponseContent = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Message = errorMessage
            };

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponseContent))
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedErrorResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = target.Delete(phoneId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }


        [Fact]
        public void Delete_ReturnsRedirectWithErrorMessage_WhenServiceResponseIsNull()
        {
            // Arrange
            int phoneId = 1;
            var errorMessage = "Someething went wrong.3Try again later.";
            var expectedErrorResponseContent = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Message = errorMessage
            };

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponseContent))
            };
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedErrorResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = target.Delete(phoneId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsRedirectWithErrorMessage_WhenErrrorResponseIsNull()
        {
            // Arrange
            int phoneId = 1;
            var errorMessage = "Someething went wrong.Try again later.";

            var expectedErrorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedErrorResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = target.Delete(phoneId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ShowAllContactWithPagination", result.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsRedirectToActionResult_WhenDeleteSucceeds()
        {
            // Arrange
            var categoryId = 1;
            var successMessage = "Contact deleted successfully.";
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            var expectedResponse = new ServiceResponse<string> { Success = true, Message = successMessage };
            mockHttpClientService
            .Setup(s => s.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), mockRequest.Object, It.IsAny<object>(), It.IsAny<int>()))
            .Returns(expectedResponse);
            var controller = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = controller.DeleteConfirm(categoryId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ShowAllContactWithPagination", redirectToActionResult.ActionName);
            Assert.Equal(successMessage, mockTempData["SuccessMessage"]);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsRedirectToActionResult_WhenDeleteNotSucceeds()
        {
            // Arrange
            var categoryId = 1;
            var errorMessage = "Someething went wrong.3Try again later.";
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            var expectedResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = "Someething went wrong.3Try again later."
            };
            mockHttpClientService
            .Setup(s => s.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), mockRequest.Object, It.IsAny<object>(), It.IsAny<int>()))
            .Returns(expectedResponse);
            var controller = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object },
                TempData = mockTempData
            };

            // Act
            var result = controller.DeleteConfirm(categoryId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ShowAllContactWithPagination", redirectToActionResult.ActionName);
            Assert.Equal(errorMessage, mockTempData["ErrorMessage"]);
        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsEmptyList_WhenLetterIsNotNull()
        {
            // Arrange
            char? letter = 'f';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> {
            new ContactViewModel { PhoneId = 1, FirstName = "firstname", Favourites=true,Email = "john@example.com" },
            };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPaginationFav(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);


        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsCorrectView_WhenSearchQueryIsNotEmpty()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { }; // Empty list as expected result

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPaginationFav(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<List<ContactViewModel>>(actual.Model);
            var model = actual.Model as List<ContactViewModel>;
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsEmptyList_WhenLetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            // Arrange
            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPaginationFav(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);


        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsEmptyView_WhenTotalCountIsZero()
        {
            // Arrange
            char? letter = 'A';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();


            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.ShowAllContactWithPaginationFav(letter, page, pageSize, sortOrder) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            var model = actual.Model as List<ContactViewModel>;
            Assert.Empty(model);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsRedirectToIndex_WhenPageIsGreaterThanTotalPages()
        {
            // Arrange
            char? letter = 'A';
            int page = 4;
            int pageSize = 2;
            string sortOrder = "asc";

            var expectedCategories = new List<ContactViewModel>();
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 3 }); // Mocking totalCount as 3

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.ShowAllContactWithPaginationFav(letter, page, pageSize, sortOrder);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ShowAllContactWithPaginationFav", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
            Assert.Equal(pageSize, redirectToActionResult.RouteValues["pageSize"]);
            Assert.Equal(letter, redirectToActionResult.RouteValues["letter"]);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsContacts_WhenSearchAndLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange
            var contacts = new List<ContactViewModel>
    {
        new ContactViewModel{ PhoneId=1, FirstName="FirstName 1", Favourites=true},
        new ContactViewModel{ PhoneId=2, FirstName="FirstName 2", Favourites=true},
    };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            // Mock for successful response
            mockHttpClientService.SetupSequence(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(),
                    null,
                    60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 4 });

            // Mock for successful response
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(),
                    null,
                    60))
                .Returns(new ServiceResponse<IEnumerable<ContactViewModel>> { Success = true, Data = contacts });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ShowAllContactWithPaginationFav('f', 1, 2, "asc") as ViewResult;

            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                It.IsAny<string>(),
                HttpMethod.Get,
                It.IsAny<HttpRequest>(),
                null,
                60),
                Times.Once);

            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                It.IsAny<string>(),
                HttpMethod.Get,
                It.IsAny<HttpRequest>(),
                null,
                60),
                Times.Once);
        }
        [Fact]
        public void ShowAllContactWithPaginationFav_ReturnsEmptyContacts_WhenSearchAndLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockImage = new Mock<IImageUpload>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();

            // Mock for successful response for count of contacts
            mockHttpClientService.SetupSequence(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(),
                    null,
                    60))
                .Returns(new ServiceResponse<int> { Success = true, Data = 4 }) // Successful response for count of contacts
                .Returns(new ServiceResponse<int> { Success = false }); // Unsuccessful response for count of contacts

            // Mock for successful response for contacts
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(),
                    null,
                    60))
                .Returns(new ServiceResponse<IEnumerable<ContactViewModel>> { Success = true, Data = new List<ContactViewModel>() });
               

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actualSuccess = target.ShowAllContactWithPaginationFav('f', 1, 2, "asc") as ViewResult; // Test for successful response
            var actualFailure = target.ShowAllContactWithPaginationFav('f', 1, 2, "asc") as ViewResult; // Test for unsuccessful response

            //Assert
           
            Assert.NotNull(actualSuccess);
            Assert.NotNull(actualFailure);
        }

       
        [Fact]
        public void Favourite_ReturnsNoContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>();
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockWebHostEnvironment.Object,mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ShowAllContactWithPaginationFav(null, 1, 2) as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
    }
}
