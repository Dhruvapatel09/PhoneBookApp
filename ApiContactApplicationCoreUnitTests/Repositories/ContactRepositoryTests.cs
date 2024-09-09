using ApiApplicationCore.Data;
using ApiApplicationCore.Data.Implementation;
using ApiApplicationCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApiContactApplicationCoreUnitTests.Repositories
{
    public class ContactRepositoryTests
    {

        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {
            // Arrange
            var contactsList = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "Company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new PhoneBookModel
            {
                PhoneId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "0987654321",
                Company = "Company",
                Image = "file2.txt",
                Gender = "Female",
                Favourites = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            Assert.Equal(contactsList, actual.ToList());
            mockDbContext.Verify(c => c.phoneBookModels, Times.Once);
        }


        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoContactsExistWhenLetterIsNull()
        {
            var contactsList = new List<PhoneBookModel> { }
      .AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(2));
        }

        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            char? letter = 'J';
            var contactsList = new List<PhoneBookModel>
            {
               new PhoneBookModel
             {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());
            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            mockDbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(2));

        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoContactsExistLetterIsNotNull()
        {
            char? letter = 'a';

            var contactsList = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());
            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockDbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(2));

        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contactsList = new List<PhoneBookModel>
            {
               new PhoneBookModel
             {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new PhoneBookModel
            {
                 PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once());
        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsEmpty_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contactsList = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contactsList = new List<PhoneBookModel>
            {
               new PhoneBookModel
             {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new PhoneBookModel
            {
                 PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.txt",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once());

        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsEmpty_WhenNoContactsExistLetterIsNotNull()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contactsList = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contactsList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(contactsList.Count(), actual.Count());
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once());

        }

        [Fact]
        public void GetPhoneBookModel_WhenPhoneBookModelIsNull()
        {
            //Arrange
            var id = 1;

            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.phoneBookModels, Times.Once);

        }
        [Fact]
        public void GetPhoneBookModel_WhenPhoneBookModelIsNotNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<PhoneBookModel>()
            {
              new PhoneBookModel { PhoneId = 1, FirstName = "PhoneBookModel 1" },
                new PhoneBookModel { PhoneId = 2, FirstName = "PhoneBookModel 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            string searchQuery = "search";
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, searchQuery);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            string searchQuery = "search";
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel> {
                new PhoneBookModel {PhoneId = 1,FirstName="Contact 1"},
                new PhoneBookModel {PhoneId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, searchQuery);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            string searchQuery = "search";
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, searchQuery);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<PhoneBookModel> {
                new PhoneBookModel {PhoneId = 1,FirstName="Contact 1"},
                new PhoneBookModel {PhoneId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<PhoneBookModel>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<PhoneBookModel> {
                new PhoneBookModel {PhoneId = 1,FirstName="Contact 1"},
                new PhoneBookModel {PhoneId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<PhoneBookModel>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalFavContacts(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.phoneBookModels, Times.Once);

        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists()
        {
            string searchQuery = "search";
            string sortOrder = "asc";
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            var contacts = new List<PhoneBookModel>
              {
                  new PhoneBookModel{PhoneId=1, FirstName="Contact 1"},
                  new PhoneBookModel{PhoneId=2, FirstName="Contact 2"},
                  new PhoneBookModel{PhoneId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists()
        {
            string searchQuery = "search";
            string sortOrder = "asc";
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter()
        {

            string searchQuery = "search";
            string sortOrder = "asc";
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            var contacts = new List<PhoneBookModel>
              {
                  new PhoneBookModel{PhoneId=1, FirstName="Contact 1"},
                  new PhoneBookModel{PhoneId=2, FirstName="Contact 2"},
                  new PhoneBookModel{PhoneId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter()
        {

            string searchQuery = "search";
            string sortOrder = "asc";
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_SortedDescending()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var letter = 't'; 
            var searchQuery = ""; 
            var sortOrder = "desc"; 

            var contactsList = new List<PhoneBookModel>
            {
                new PhoneBookModel{PhoneId=1, FirstName="test 1"},
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.Count());
            var sorted = actual.OrderByDescending(c => c.FirstName).ToList();
            Assert.Equal(sorted, actual);
        }

        [Fact]
        public void GetPaginatedContacts_ThrowsArgumentException_WhenInvalidSortingOrder()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var letter = 't';
            var searchQuery = "";
            var invalidSortOrder = "invalid";

            var contactsList = new List<PhoneBookModel>
            {
               new PhoneBookModel{PhoneId=1, FirstName="test 1"},
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => target.GetPaginatedContacts(page, pageSize, letter, searchQuery, invalidSortOrder));
        }


        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExists()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var contacts = new List<PhoneBookModel>
              {
                  new PhoneBookModel{PhoneId=1, FirstName="Contact 1"},
                  new PhoneBookModel{PhoneId=2, FirstName="Contact 2"},
                  new PhoneBookModel{PhoneId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExists()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>
              {
                  new PhoneBookModel{PhoneId=1, FirstName="Contact 1"},
                  new PhoneBookModel{PhoneId=2, FirstName="Contact 2"},
                  new PhoneBookModel{PhoneId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(1, 2, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter()
        {
            char? letter = 'D';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        public void GetFavouriteContacts_ReturnsContacts_SortedDescending()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var letter = 't'; 
            var sortOrder = "desc"; 

            var contactsList = new List<PhoneBookModel>
    {
        new PhoneBookModel{ PhoneId = 1, FirstName = "test 1",Favourites=true },
        new PhoneBookModel{ PhoneId = 2, FirstName = "test 2",Favourites=true  },
        new PhoneBookModel{ PhoneId = 3, FirstName = "test 3" ,Favourites=false },
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            var sorted = actual.OrderByDescending(c => c.FirstName).ToList();
            Assert.Equal(sorted, actual); 
        }


        [Fact]
        public void GetFavouriteContacts_ReturnsContacts_DefaultSorting()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var letter = 'J'; 
            var sortOrder = ""; 

            var contactsList = new List<PhoneBookModel>
            {
                  new PhoneBookModel{ PhoneId = 1, FirstName = "test 1",Favourites=true },
        new PhoneBookModel{ PhoneId = 2, FirstName = "test 2",Favourites=true  },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

         
           
            Assert.Throws<ArgumentException>(() => target.GetFavouriteContacts(page, pageSize, letter, sortOrder));
        }

        [Fact]
        public void InsertContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.phoneBookModels).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.InsertContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertContact_ReturnsFalse()
        {
            //Arrange
            PhoneBookModel contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.InsertContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void UpdateContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.phoneBookModels).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.UpdateContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateContact_ReturnsFalse()
        {
            //Arrange
            PhoneBookModel contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void DeleteContact_ReturnsTrue()
        {
            // Arrange
            var PhoneId = 1;
            var contact = new PhoneBookModel { PhoneId = PhoneId };
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.phoneBookModels.Find(PhoneId)).Returns(contact);
            var target = new ContactRepository(mockContext.Object);
            // Act
            var result = target.DeleteContact(PhoneId);

            // Assert
            Assert.True(result);
            mockContext.Verify(c => c.phoneBookModels.Remove(contact), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            mockContext.Verify(c => c.phoneBookModels.Find(PhoneId), Times.Once);

        }

        [Fact]
        public void DeleteContact_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteContact(id);
            //Assert
            Assert.False(actual);
            mockAbContext.VerifyGet(c => c.phoneBookModels, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<PhoneBookModel>
            {
                new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber="1234567890"},
                new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2", PhoneNumber="9876543216" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
        }
        [Fact]
        public void ContactExistsIdName_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var id = 1;
            var contacts = new List<PhoneBookModel>().AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
        }
        [Fact]
        public void ContactExistsIdName_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var id = 3;
            var contacts = new List<PhoneBookModel>
            {
                new PhoneBookModel { PhoneId = 1, FirstName = "Contact 1", PhoneNumber="1234567890" },
                new PhoneBookModel { PhoneId = 2, FirstName = "Contact 2" , PhoneNumber="9876543219"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.phoneBookModels, Times.Once);
        }
        [Fact]
        public void GetByLetter_ReturnsContacts_WhenContactsExistForLetter()
        {
            // Arrange
            var letter = 'j';
            var contactsList = new List<PhoneBookModel>
    {
        new PhoneBookModel
        {
            PhoneId = 1,
            FirstName = "john",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Company = "Company",
            Image = "file1.txt",
            Gender = "Male",
            Favourites = true,
            CountryId = 1,
            StateId = 1,
            Country = new Country
            {
                CountryId = 1,
                CountryName = "USA"
            },
            State = new State
            {
                StateId = 1,
                StateName = "California"
            }
        },
        new PhoneBookModel
        {
            PhoneId = 2,
            FirstName = "jane",
            LastName = "Smith",
            Email = "jane@example.com",
            PhoneNumber = "0987654321",
            Company = "Company",
            Image = "file2.txt",
            Gender = "Female",
            Favourites = false,
            CountryId = 2,
            StateId = 2,
            Country = new Country
            {
                CountryId = 2,
                CountryName = "Canada"
            },
            State = new State
            {
                StateId = 2,
                StateName = "Ontario"
            }
        }
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetByLetter(letter);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            Assert.All(actual, c => Assert.StartsWith(letter.ToString(), c.FirstName));
        }

        [Fact]
        public void GetByLetter_ReturnsEmptyList_WhenNoContactsExistForLetter()
        {
            // Arrange
            var letter = 'z';
            var emptyContactsList = new List<PhoneBookModel>().AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(emptyContactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(emptyContactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(emptyContactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(emptyContactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetByLetter(letter);

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExistForLetter()
        {
            // Arrange
            var letter = 'j';
            var contactsList = new List<PhoneBookModel>
    {
        new PhoneBookModel
        {
            PhoneId = 1,
            FirstName = "john",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Company = "Company",
            Image = "file1.txt",
            Gender = "Male",
            Favourites = true,
            CountryId = 1,
            StateId = 1,
            Country = new Country
            {
                CountryId = 1,
                CountryName = "USA"
            },
            State = new State
            {
                StateId = 1,
                StateName = "California"
            }
        },
        new PhoneBookModel
        {
            PhoneId = 2,
            FirstName = "jane",
            LastName = "Smith",
            Email = "jane@example.com",
            PhoneNumber = "0987654321",
            Company = "Company",
            Image = "file2.txt",
            Gender = "Female",
            Favourites = false,
            CountryId = 2,
            StateId = 2,
            Country = new Country
            {
                CountryId = 2,
                CountryName = "Canada"
            },
            State = new State
            {
                StateId = 2,
                StateName = "Ontario"
            }
        }
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(contactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(contactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(contactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(contactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.Count());
            Assert.All(actual, c => Assert.StartsWith(letter.ToString(), c.FirstName));
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsEmptyList_WhenNoContactsExistForLetter()
        {
            // Arrange
            var letter = 'z';
            var emptyContactsList = new List<PhoneBookModel>().AsQueryable();

            var mockDbSet = new Mock<DbSet<PhoneBookModel>>();
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Provider).Returns(emptyContactsList.Provider);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.Expression).Returns(emptyContactsList.Expression);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.ElementType).Returns(emptyContactsList.ElementType);
            mockDbSet.As<IQueryable<PhoneBookModel>>().Setup(m => m.GetEnumerator()).Returns(emptyContactsList.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(c => c.phoneBookModels).Returns(mockDbSet.Object);

            var target = new ContactRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter);

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }
    }
}
