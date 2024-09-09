using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Implementation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContactApplicationCoreUnitTests.Services
{
    public class ContactServiceTests
    {
        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            var contacts = new List<PhoneBookModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';

            // Arrange
            var contacts = new List<PhoneBookModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
               PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            string sortOrder = "asc";
            int pageSize = 2;
            var contacts = new List<PhoneBookModel>();
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            int page = 1;
            int pageSize = 2;

            string sortOrder = "asc";
            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            // Arrange
            var contacts = new List<PhoneBookModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
               PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }

        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
               PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }

       
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            string sortOrder = "asc";

            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };
            int page = 1;
            int pageSize = 2;
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var letter = 'x';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNull()
        {
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John"

            },
            new PhoneBookModel
            {
                PhoneId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavContacts(null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavContacts(null), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNotNull()
        {
            var letter = 'c';
            var contacts = new List<PhoneBookModel>
        {
            new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John"

            },
            new PhoneBookModel
            {
                PhoneId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavContacts(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavContacts(letter), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsContact_WhenContactExist()
        {
            // Arrange
            var PhoneId = 1;
            var contact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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

            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(PhoneId)).Returns(contact);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(PhoneId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetContact(PhoneId), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsNoRecord_WhenNoContactsExist()
        {
            // Arrange
            var PhoneId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(PhoneId)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(PhoneId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal("No records found.", actual.Message);
            mockRepository.Verify(r => r.GetContact(PhoneId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactSavedSuccessfully_WhenContactisSaved()
        {
            var contact = new PhoneBookModel()
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,

            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.PhoneNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(true);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Contact saved successfully.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.PhoneNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsSomethingWentWrong_WhenContactisNotSaved()
        {
            var contact = new PhoneBookModel()
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.PhoneNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(false);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.PhoneNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contact = new PhoneBookModel()
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.PhoneNumber)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Already eists", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.PhoneNumber), Times.Once);

        }
        [Fact]
        public void AddContact_SetsDefaultImage_WhenImageIsNullOrEmpty()
        {
            // Arrange
            var contact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "Company",
                Image = null,
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
            };

            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.ContactExists(It.IsAny<string>())).Returns(false);
            mockContactRepository.Setup(repo => repo.InsertContact(It.IsAny<PhoneBookModel>())).Returns(true);

            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.AddContact(contact);

            // Assert
            Assert.True(response.Success); 
            Assert.Equal("DefaultImage.jpg", contact.Image); 
        }

        [Fact]
        public void ModifyContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var PhoneId = 1;
            var contact = new PhoneBookModel()
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(PhoneId, contact.PhoneNumber)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact already exists.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(PhoneId, contact.PhoneNumber), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsSomethingWentWrong_WhenContactNotFound()
        {
            var PhoneId = 1;
            var existingContact = new PhoneBookModel()
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,

            };

            var updatedContact = new PhoneBookModel()
            {
                PhoneId = PhoneId,
                FirstName = "C1"
            };


            var mockRepository = new Mock<IContactRepository>();
            //mockRepository.Setup(r => r.ContactExist(PhoneId, updatedContact.Phone)).Returns(false);
            mockRepository.Setup(r => r.GetContact(updatedContact.PhoneId)).Returns<IEnumerable<PhoneBookModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(existingContact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            //mockRepository.Verify(r => r.ContactExist(PhoneId, updatedContact.Phone), Times.Once);
            mockRepository.Verify(r => r.GetContact(PhoneId), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsUpdatedSuccessfully_WhenContactModifiedSuccessfully()
        {

            //Arrange
            var existingContact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };

            var updatedContact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.PhoneId, updatedContact.PhoneNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.PhoneId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(true);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Contact updated successfully.", actual.Message);

            mockContactRepository.Verify(c => c.GetContact(updatedContact.PhoneId), Times.Once);


            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }
        [Fact]
        public void ModifyContact_ReturnsError_WhenContactModifiedFails()
        {

            //Arrange
            var existingContact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };

            var updatedContact = new PhoneBookModel
            {
                PhoneId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.PhoneId, updatedContact.PhoneNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.PhoneId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(false);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockContactRepository.Verify(c => c.GetContact(updatedContact.PhoneId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var PhoneId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(PhoneId)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(PhoneId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Contact deleted successfully.", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(PhoneId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var PhoneId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(PhoneId)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(PhoneId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(PhoneId), Times.Once);
        }
        [Fact]
        public void GetContactByLetter_ReturnsContacts_WhenContactsExistForLetter()
        {
            // Arrange
            char letter = 'j';
            var contacts = new List<PhoneBookModel>
            {new PhoneBookModel
            {
               PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetByLetter(It.IsAny<char>())).Returns(contacts);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetContactByLetter(letter);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
           
        }
        [Fact]
        public void GetContactByLetter_ReturnsNoRecordsFound_WhenNoContactsExistForLetter()
        {
            // Arrange
            char letter = 'Z'; 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetByLetter(It.IsAny<char>())).Returns((IQueryable<PhoneBookModel>)null);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetContactByLetter(letter);

            // Assert
            Assert.False(response.Success);
            Assert.Empty(response.Data);
            Assert.Equal("No record found.", response.Message);
        }
        [Fact]
        public void TotalContacts_ReturnsCorrectCount_WhenContactsExistForLetterAndSearchQuery()
        {
            // Arrange
            char? letter = 'a'; 
            string searchQuery = "example"; 
            int expectedCount = 5; 

            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.TotalContacts(It.IsAny<char?>(), It.IsAny<string?>())).Returns(expectedCount);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.TotalContacts(letter, searchQuery);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(expectedCount, response.Data);
            Assert.Equal("Pagination successful", response.Message);
        }
        [Fact]
        public void TotalContacts_ReturnsZeroCount_WhenNoContactsExistForLetterAndSearchQuery()
        {
            // Arrange
            char? letter = 'a';
            string searchQuery = "nonexistent"; 
            int expectedCount = 0; 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.TotalContacts(It.IsAny<char?>(), It.IsAny<string?>())).Returns(expectedCount);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.TotalContacts(letter, searchQuery);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(expectedCount, response.Data);
            Assert.Equal("Pagination successful", response.Message);
        }
        [Fact]
        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistForLetter()
        {
            // Arrange
            char letter = 'j';
            var contacts = new List<PhoneBookModel>
            {new PhoneBookModel
            {
               PhoneId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Company = "company",
                Image = "file1.png",
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
                Image = "file1.png",
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
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetAllFavouriteContacts(It.IsAny<char>())).Returns(contacts);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetFavouriteContacts(letter);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);

        }
        [Fact]
        public void GetFavouriteContacts_ReturnsNoRecordsFound_WhenNoContactsExistForLetter()
        {
            // Arrange
            char letter = 'Z';
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetAllFavouriteContacts(It.IsAny<char>())).Returns((IQueryable<PhoneBookModel>)null);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetFavouriteContacts(letter);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("No record found", response.Message);
        }

    }
}
