using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;

namespace ApiApplicationCore.Services.Contract
{
    public interface IContactService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContact();

        ServiceResponse<string> AddContact(PhoneBookModel contact);

        ServiceResponse<string> RemoveContact(int id);

        ServiceResponse<string> ModifyContact(PhoneBookModel contact);
        ServiceResponse<ContactDto> GetContact(int contactId);
        ServiceResponse<IEnumerable<ContactDto>> GetContactByLetter(char letter);
        //ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize);
        //ServiceResponse<int> TotalContacts();
        ServiceResponse<int> TotalContacts(char? letter, string? searchQuery);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter, string? searchQuery, string sortOrder);
        ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(int page, int pageSize, char? letter, string sortOrder);
        ServiceResponse<int> TotalFavContacts(char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter);
    }
}
