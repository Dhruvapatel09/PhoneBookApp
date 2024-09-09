using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiApplicationCore.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _AppDBContext;

        public ContactRepository(IAppDbContext AppDBContext)
        {
            _AppDBContext = AppDBContext;
        }
        public IEnumerable<PhoneBookModel> GetAll()
        {
            List<PhoneBookModel> contacts = _AppDBContext.phoneBookModels.Include(c=>c.State).Include(c=>c.Country).ToList();
            return contacts;
        }
        public IEnumerable<PhoneBookModel> GetByLetter(char letter)
        {
            var contacts = _AppDBContext.phoneBookModels.Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
           
                return contacts;
            
        }

        public int TotalContacts(char? letter, string? searchQuery)
        {
            IQueryable<PhoneBookModel> query = _AppDBContext.phoneBookModels;

            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.FirstName.Contains(searchQuery) || c.LastName.Contains(searchQuery));
            }

            return query.Count();
        }
        public int TotalFavContacts(char? letter)
        {
            IQueryable<PhoneBookModel> query = _AppDBContext.phoneBookModels.Where(c => c.Favourites);
            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }

        public IEnumerable<PhoneBookModel> GetPaginatedContacts(int page, int pageSize, char? letter, string? searchQuery, string sortOrder)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<PhoneBookModel> query = _AppDBContext.phoneBookModels
                .Include(c => c.State)
                .Include(c => c.Country);

            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));

            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.FirstName.Contains(searchQuery) || c.LastName.Contains(searchQuery));
            }

            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
                default:
                    throw new ArgumentException("Invalid sorting order");
            }

            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public PhoneBookModel? GetContact(int id)
        {
            var contact = _AppDBContext.phoneBookModels.Include(c => c.State).Include(c => c.Country).FirstOrDefault(c => c.PhoneId == id);
            return contact;
        }
        public bool InsertContact(PhoneBookModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _AppDBContext.phoneBookModels.Add(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool UpdateContact(PhoneBookModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _AppDBContext.phoneBookModels.Update(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _AppDBContext.phoneBookModels.Find(id);
            if (contact != null)
            {
                _AppDBContext.phoneBookModels.Remove(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }

            return result;
        }
        public IEnumerable<PhoneBookModel> GetFavouriteContacts(int page, int pageSize, char? letter, string sortOrder)
        {
            int skip = (page - 1) * pageSize;

            // Trim the search query to remove extra spaces

            // Build the query with the necessary filters
            var query = _AppDBContext.phoneBookModels.Include(c => c.State).Include(c => c.Country).AsQueryable();

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.Value.ToString()));
            }

            // Apply sorting based on sortOrder
            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName);
                    break;
                default:
                    throw new ArgumentException("Invalid sorting order");
            }

            // Apply pagination
            return query
                .Where(c=> c.Favourites)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public bool ContactExists(string contactNumber)
        {
            var contact = _AppDBContext.phoneBookModels.FirstOrDefault(c => c.PhoneNumber == contactNumber);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int phoneId,string contactNumber)
        {
            var contact = _AppDBContext.phoneBookModels.FirstOrDefault(c => c.PhoneId != phoneId && c.PhoneNumber == contactNumber);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<PhoneBookModel> GetAllFavouriteContacts(char? letter)
        {

            List<PhoneBookModel> contacts = _AppDBContext.phoneBookModels
                .Include(c => c.Country)
                .Include(c => c.State)
                .Where(c => c.Favourites)
                .Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return contacts;
        }

    }
}
