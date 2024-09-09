using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Data.Contract;
using PhoneBookApp.Models;
using System;

namespace PhoneBookApp.Data.Implementation
{
    public class ContactRepository:IContactRepository
    {
        private readonly AppDbContext _appDbContext;

        public ContactRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<PhoneBookModel> GetAll()
        {
            List<PhoneBookModel> contacts = _appDbContext.phoneBookModels.ToList();
            return contacts;
        }
        public int TotalContacts()
        {
            return _appDbContext.phoneBookModels.Count();
        }
        public IEnumerable<PhoneBookModel> GetPaginatedContact(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.phoneBookModels
                .OrderBy(c => c.PhoneId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public PhoneBookModel? GetContact(int id)
        {
            var contact = _appDbContext.phoneBookModels.FirstOrDefault(c => c.PhoneId == id);
            return contact;
        }
        public bool InsertContact(PhoneBookModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.phoneBookModels.Add(contact);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool UpdateContact(PhoneBookModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _appDbContext.phoneBookModels.Update(contact);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _appDbContext.phoneBookModels.Find(id);
            if (contact != null)
            {
                _appDbContext.phoneBookModels.Remove(contact);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool ContactExists(string name)
        {
            var contact = _appDbContext.phoneBookModels.FirstOrDefault(c => c.FirstName == name);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int phoneId, string name)
        {
            var contact = _appDbContext.phoneBookModels.FirstOrDefault(c => c.PhoneId != phoneId && c.FirstName == name);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
