using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplicationCore.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetContact()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contact = _contactRepository.GetAll();
            if (contact != null && contact.Any())
            {
                contact.Where(c => c.Image == string.Empty).ToList();
                List<ContactDto> contactDto = new List<ContactDto>();

                foreach (var contacts in contact)
                {
                    contactDto.Add(new ContactDto() { 
                        PhoneId = contacts.PhoneId, 
                        FirstName = contacts.FirstName, 
                        LastName = contacts.LastName, 
                        Email = contacts.Email, 
                        PhoneNumber = contacts.PhoneNumber, 
                        Company = contacts.Company ,
                        Image=contacts.Image,
                        Gender = contacts.Gender,
                        Favourites = contacts.Favourites,
                        StateId = contacts.StateId,
                        ImageByte = contacts.ImageByte,
                        CountryId = contacts.CountryId,
                        Birthdate = contacts.Birthdate,
                        State =new State()
                        {
                            StateId=contacts.State.StateId,
                            StateName=contacts.State.StateName,
                            CountryId=contacts.Country.CountryId,
                        },
                        Country=new Country()
                        {
                            CountryId=contacts.CountryId,
                            CountryName=contacts.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDto;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }

        public ServiceResponse<string> AddContact(PhoneBookModel contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.PhoneNumber))
            {
                response.Success = false;
                response.Message = "Contact Already eists";
                return response;
            }

            var fileName = string.Empty;
            if (string.IsNullOrEmpty(contact.Image))
            {
                contact.Image = "DefaultImage.jpg";
            }
            var result = _contactRepository.InsertContact(contact);
            if (result)
            {
                response.Message = "Contact saved successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }

        public ServiceResponse<string> ModifyContact(PhoneBookModel contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.PhoneId, contact.PhoneNumber))
            {
                response.Success = false;
                response.Message = "Contact already exists.";
                return response;
            }
            var fileName = string.Empty;
            if (string.IsNullOrEmpty(contact.Image))
            {
                contact.Image = "DefaultImage.jpg";
            }
            var existingContact = _contactRepository.GetContact(contact.PhoneId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.PhoneNumber = contact.PhoneNumber;
                existingContact.Email = contact.Email;
                existingContact.Company = contact.Company;
                existingContact.Image=contact.Image;
                existingContact.Gender = contact.Gender;
                existingContact.Favourites = contact.Favourites;
                existingContact.CountryId = contact.CountryId;
                existingContact.ImageByte = contact.ImageByte;
                existingContact.StateId = contact.StateId;
                existingContact.Birthdate = contact.Birthdate;
                result = _contactRepository.UpdateContact(existingContact);
            }

            if (result)
            {
                response.Message = "Contact updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                response.Message = "Contact deleted successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }
        public ServiceResponse<ContactDto> GetContact(int contactId)
        {
            var response = new ServiceResponse<ContactDto>();
            var contact = _contactRepository.GetContact(contactId);
            if (contact == null)
            {
                response.Success = false;
                response.Data = new ContactDto();
                response.Message = "No records found.";
                return response;
            }
            var contactsDtos = new ContactDto()
            {

                PhoneId = contact.PhoneId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Company = contact.Company,
                Image = contact.Image,
                Gender = contact.Gender,
                Favourites = contact.Favourites,
                StateId = contact.StateId,
                ImageByte = contact.ImageByte,
                CountryId = contact.CountryId,
                Birthdate = contact.Birthdate,
                State = new State()
                {
                    StateId = contact.State.StateId,
                    StateName = contact.State.StateName,
                    CountryId = contact.Country.CountryId,
                },
                Country = new Country()
                {
                    CountryId = contact.CountryId,
                    CountryName = contact.Country.CountryName,
                }
            };


            response.Data = contactsDtos;
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetContactByLetter(char letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetByLetter(letter);
            if (contacts == null)
            {
                response.Success = false;
                response.Data = new List<ContactDto>();
                response.Message = "No record found.";
                return response;
            }
            List<ContactDto> contactsDtos = new List<ContactDto>();
            foreach (var contact in contacts.ToList())
            {
                contactsDtos.Add(
                    new ContactDto()
                    {
                        PhoneId = contact.PhoneId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        PhoneNumber = contact.PhoneNumber,
                        Company = contact.Company,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        ImageByte = contact.ImageByte,
                        Birthdate = contact.Birthdate,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.Country.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });

            }

            response.Data = contactsDtos;
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter,string? searchQuery, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, letter,searchQuery,sortOrder);

            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        PhoneId = contact.PhoneId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        PhoneNumber = contact.PhoneNumber,
                        Company = contact.Company,
                        Image = contact.Image,
                        Email = contact.Email,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        ImageByte = contact.ImageByte,
                        Birthdate = contact.Birthdate,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.Country.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }


                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<int> TotalContacts(char? letter, string? searchQuery)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContacts(letter,searchQuery);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Pagination successful";

            return response;
        }
        public ServiceResponse<int> TotalFavContacts(char? letter)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalFavContacts(letter);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Pagination successful";

            return response;
        }
        //public ServiceResponse<int> TotalContacts()
        //{
        //    var response = new ServiceResponse<int>();

        //    var result = _contactRepository.TotalContacts();
        //    response.Data = result;
        //    response.Success = true;

        //    return response;
        //}
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(int page, int pageSize, char? letter, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            if (contacts != null && contacts.Any())
            {
                var favouriteContacts = contacts.Where(c => c.Favourites).ToList();
                if (favouriteContacts.Any())
                {
                    var contactDtoList = new List<ContactDto>();

                    foreach (var contact in favouriteContacts)
                    {
                        contactDtoList.Add(new ContactDto()
                        {
                            PhoneId = contact.PhoneId,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email,
                            PhoneNumber = contact.PhoneNumber,
                            Company = contact.Company,
                            Image = contact.Image,
                            Gender = contact.Gender,
                            Favourites = contact.Favourites,
                            StateId = contact.StateId,
                            CountryId = contact.CountryId,
                            ImageByte = contact.ImageByte,
                            Birthdate = contact.Birthdate,
                            State = new State()
                            {
                                StateId = contact.State.StateId,
                                StateName = contact.State.StateName,
                                CountryId = contact.State.CountryId,
                            },
                            Country = new Country()
                            {
                                CountryId = contact.CountryId,
                                CountryName = contact.Country.CountryName,
                            }
                        });
                    }
                    response.Data = contactDtoList;
                    response.Success = true;
                    return response;
                }
            }
            response.Success = false;
            response.Message = "No favourite contacts found!";
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllFavouriteContacts(letter);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.Image == string.Empty).ToList();
                List<ContactDto> contactDtos = new List<ContactDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        PhoneId = contact.PhoneId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        PhoneNumber = contact.PhoneNumber,
                        Company = contact.Company,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        ImageByte = contact.ImageByte,
                        Birthdate = contact.Birthdate,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }

    }
}
