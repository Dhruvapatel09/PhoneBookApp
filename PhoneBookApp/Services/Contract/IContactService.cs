using PhoneBookApp.Models;
using PhoneBookApp.ViewModel;

namespace PhoneBookApp.Services.Contract
{
    public interface IContactService
    {
        IEnumerable<PhoneBookModel> GetContact();
        int TotalContacts();
        IEnumerable<PhoneBookModel> GetPaginatedContact(int page, int pageSize);

        PhoneBookModel? GetContact(int id);

        string AddContact(PhoneBookModel contact, IFormFile file);

        string RemoveContact(int id);

        string ModifyContact(PhoneBookModel contact);

    }
}
