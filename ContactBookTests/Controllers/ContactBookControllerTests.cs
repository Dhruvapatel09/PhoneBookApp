using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBookApp.Controllers;
using PhoneBookApp.Models;
using PhoneBookApp.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookTests.Controllers
{
    public class ContactBookControllerTests
    {
        [Fact]
        public void Index1_ReturnsViewWithNull_WhenNoContactIsNull()
        {
            //Arrange
            
             PhoneBookModel categories = new PhoneBookModel();
            var mockCategoryService = new Mock<IContactService>();
            var target = new ContactController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetContact()).Returns((List<PhoneBookModel>)null);

            //Act
            var actual = target.Index1() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal("ContactList", actual.ViewName);
            mockCategoryService.Verify(c => c.GetContact(), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsViewWithEmptyList_WhenNoCategoryExists()
        {
            // Arrange


            PhoneBookModel categories = new PhoneBookModel
             {
                PhoneId=1,FirstName="FName 1",LastName="Lname 1"
             };
            var mockCategoryService = new Mock<IContactService>();
            var target = new ContactController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetContact()).Returns((new List<PhoneBookModel>()));

            //Act
            var actual = target.Index1() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Model);
            Assert.Equal("ContactList", actual.ViewName);
            mockCategoryService.Verify(c => c.GetContact(), Times.Once);

        }
    }
}
