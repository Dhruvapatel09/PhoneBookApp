using ApiApplicationCore.Models;

namespace ApiApplicationCore.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<PhoneBookModel> GetAll();
   
        int TotalContacts(char? letter, string? searchQuery);

        PhoneBookModel? GetContact(int id);
        IEnumerable<PhoneBookModel> GetByLetter(char letter);
        bool ContactExists(string contactNumber);

        bool ContactExists(int phoneId, string contactNumber);

        bool InsertContact(PhoneBookModel contact);

        bool UpdateContact(PhoneBookModel contact);

        bool DeleteContact(int id);
        IEnumerable<PhoneBookModel> GetFavouriteContacts(int page, int pageSize, char? letter, string sortOrder);
        int TotalFavContacts(char? letter);
        IEnumerable<PhoneBookModel> GetPaginatedContacts(int page, int pageSize, char? letter, string? searchQuery, string sortOrder);
        IEnumerable<PhoneBookModel> GetAllFavouriteContacts(char? letter);
    }
}
