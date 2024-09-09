using ApiApplicationCore.Controllers;
using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContactApplicationCoreUnitTests.Controllers
{
    public class ContactControllerTests
    {
        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var letter = 'a';

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as OkObjectResult; // No need to pass the letter parameter

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>();

            var letter = 'a';

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>(); // Empty list

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = null, // Setting data to null when no contacts are found
                Message = "No contacts found" // Add a message indicating no contacts found
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            // Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }

    //    [Fact]
    //    public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
    //    {
    //        // Arrange
    //        char? letter = null;
    //        int page = 1;
    //        int pageSize = 2;
    //        string sortOrder = "asc";
    //        var contacts = new List<PhoneBookModel>
    //{
    //    new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
    //    new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    //};

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = true,
    //            Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, It.IsAny<char?>(), sortOrder)).Returns(response); // Setup with any value for letter

    //        // Act
    //        var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

    //        // Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
    //    }


    //    [Fact]
    //    public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
    //    {
    //        //Arrange
    //        var contacts = new List<PhoneBookModel>
    //         {
    //        new PhoneBookModel{PhoneId=1,FirstName="Contact 1", PhoneNumber= "1234567890"},
    //        new PhoneBookModel{PhoneId=2,FirstName="Contact 2", PhoneNumber= "1234567899"},
    //        };

    //        var letter = 'a';
    //        int page = 1;
    //        int pageSize = 2;
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = true,
    //            Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

    //        //Act
    //        var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

    //        //Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
    //    }
    //    [Fact]
    //    public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
    //    {
    //        //Arrange
    //        var contacts = new List<PhoneBookModel>
    //         {
    //        new PhoneBookModel{PhoneId=1,FirstName="Contact 1", PhoneNumber= "1234567890"},
    //        new PhoneBookModel{PhoneId=2,FirstName="Contact 2", PhoneNumber= "1234567899"},
    //        };

    //        var letter = 'a';
    //        int page = 1;
    //        int pageSize = 2;
    //        string sortOrder = "asc";


    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = false,
    //            Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

    //        //Act
    //        var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

    //        //Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(404, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
    //    }

    //    [Fact]
    //    public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
    //    {
    //        //Arrange
    //        var contacts = new List<PhoneBookModel>
    //         {
    //        new PhoneBookModel{PhoneId=1,FirstName="Contact 1", PhoneNumber= "1234567890"},
    //        new PhoneBookModel{PhoneId=2,FirstName="Contact 2", PhoneNumber= "1234567899"},
    //        };
    //        var letter = 'a';
    //        int page = 1;
    //        int pageSize = 2;
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = false,
    //            Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

    //        //Act
    //        var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

    //        //Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(404, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
    //    }

        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel{PhoneId=1, FirstName="Contact 1", PhoneNumber= "1234567890"},
        new PhoneBookModel{PhoneId=2, FirstName="Contact 2", PhoneNumber= "1234567899"},
    };

            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, searchQuery, sortOrder)).Returns(response);

            // Act
            var actual = target.GetPaginatedContacts(null, page, pageSize, searchQuery, sortOrder) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, searchQuery, sortOrder), Times.Once);
        }


        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetPaginatedContacts(letter, page, pageSize, searchQuery, sortOrder) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }


        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null;  // Ensure letter is null
            string searchQuery = "search";
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetPaginatedContacts(letter, page, pageSize, searchQuery, sortOrder) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }


        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetPaginatedContacts(letter, page, pageSize, searchQuery, sortOrder) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }


        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null; // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null;  // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }


        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<PhoneBookModel>
            {
               new PhoneBookModel{PhoneId=1,FirstName="Contact 1", PhoneNumber= "1234567890"},
                 new PhoneBookModel{PhoneId=2,FirstName="Contact 2", PhoneNumber= "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            //Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };
            var searchQuery = "search";
            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, searchQuery)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, searchQuery) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, searchQuery), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = 'd';
            var searchQuery = "search";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter,searchQuery)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter, searchQuery) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, searchQuery), Times.Once);
        }


        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            var searchQuery = "search";
            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, searchQuery)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter,searchQuery) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, searchQuery), Times.Once);
        }


        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            var searchQuery = "search";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null,searchQuery)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, searchQuery) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, searchQuery), Times.Once);
        }


        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfFavContacts(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalFavContacts(null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var letter = 'd';
            var totalCount = 10; // For example, total count of favorite contacts

            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = totalCount
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(letter)).Returns(response);

            // Act
            var actual = target.GetTotalCountOfFavContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(totalCount, response.Data); // Ensure data in the response is as expected
            mockContactService.Verify(c => c.TotalFavContacts(letter), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(letter)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfFavContacts(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalFavContacts(letter), Times.Once);
        }


        [Fact]

        public void GetContactById_ReturnsOk()
        {

            var PhoneId = 1;
            var contact = new PhoneBookModel
            {

                PhoneId = PhoneId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = true,
                Data = new ContactDto
                {
                    PhoneId = PhoneId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(PhoneId)).Returns(response);

            //Act
            var actual = target.GetContactById(PhoneId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(PhoneId), Times.Once);
        }

        [Fact]

        public void GetContactById_ReturnsNotFound()
        {

            var PhoneId = 1;
            var contact = new PhoneBookModel
            {

                PhoneId = PhoneId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = false,
                Data = new ContactDto
                {
                    PhoneId = PhoneId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(PhoneId)).Returns(response);

            //Act
            var actual = target.GetContactById(PhoneId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(PhoneId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsOk_WhenContactIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<PhoneBookModel>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<PhoneBookModel>()), Times.Once);

        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenContactIsNotAdded()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<PhoneBookModel>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<PhoneBookModel>()), Times.Once);

        }
        [Fact]
        public void AddContact_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidContactDto = new AddContactDto(); // This will be invalid as it has no data

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            target.ModelState.AddModelError("FirstName", "FirstName is required"); // Adding ModelState error manually

            // Act
            var actual = target.AddContact(invalidContactDto) as BadRequestObjectResult;

            // Assert
            mockContactService.Verify(c => c.AddContact(It.IsAny<PhoneBookModel>()), Times.Never); // Verify that the service method was not called
        }

        [Fact]
        public void UpdateContact_ReturnsOk_WhenContactIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<PhoneBookModel>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<PhoneBookModel>()), Times.Once);

        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<PhoneBookModel>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<PhoneBookModel>()), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {

            var PhoneId = 1;
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(PhoneId)).Returns(response);

            //Act

            var actual = target.RemoveContact(PhoneId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(PhoneId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactNotDeleted()
        {

            var PhoneId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(PhoneId)).Returns(response);

            //Act

            var actual = target.RemoveContact(PhoneId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(PhoneId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactIsLessThanZero()
        {

            var PhoneId = 0;

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);

            //Act

            var actual = target.RemoveContact(PhoneId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data.", actual.Value);
        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null; // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>
    {
        new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber = "1234567890" },
        new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null;  // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }


        [Fact]
        public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<PhoneBookModel>
            {
               new PhoneBookModel{PhoneId=1,FirstName="Contact 1", PhoneNumber= "1234567890"},
                 new PhoneBookModel{PhoneId=2,FirstName="Contact 2", PhoneNumber= "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { PhoneId = c.PhoneId, FirstName = c.FirstName, PhoneNumber = c.PhoneNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            //Act
            var actual = target.GetAllFavouriteContacts(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

    }
}
