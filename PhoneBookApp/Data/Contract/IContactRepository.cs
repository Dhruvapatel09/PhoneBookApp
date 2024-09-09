using PhoneBookApp.Models;
using System;

namespace PhoneBookApp.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<PhoneBookModel> GetAll();
        int TotalContacts();
        IEnumerable<PhoneBookModel> GetPaginatedContact(int page, int pageSize);
        PhoneBookModel? GetContact(int id);

        bool ContactExists(string name);

        bool ContactExists(int phoneId, string name);

        bool InsertContact(PhoneBookModel contact);

        bool UpdateContact(PhoneBookModel contact);

        bool DeleteContact(int id);
    }
}
