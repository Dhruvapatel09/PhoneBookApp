using PhoneBookApp.Data.Contract;
using PhoneBookApp.Models;
using PhoneBookApp.Services.Contract;
using System;
using System.Security.Policy;
namespace PhoneBookApp.Services.Implementation
{
    public class ContactService: IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<PhoneBookModel> GetContact()
        {
            var categories = _contactRepository.GetAll();
            if (categories != null && categories.Any())
            {
                //foreach (var category in categories.Where(c => c.FileName == string.Empty))
                //{
                //    category.FileName = "DefaultImage.png";
                //}
                return categories;
            }

            return new List<PhoneBookModel>();
        }

        public PhoneBookModel? GetContact(int id)
        {
            var category = _contactRepository.GetContact(id);
            return category;
        }
        public int TotalContacts()
        {
            return _contactRepository.TotalContacts();
        }
      
        public IEnumerable<PhoneBookModel> GetPaginatedContact(int page, int pageSize)
        {
            return _contactRepository.GetPaginatedContact(page, pageSize);
        }

        public string AddContact(PhoneBookModel contact, IFormFile file)
        {
            if (_contactRepository.ContactExists(contact.FirstName))
            {
                return "Contact already exists.";
            }
            var fileName = string.Empty;
            if (file != null && file.Length > 0)
            {
                //process the uploaded file (eg. save it to disk)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                //save file to storage and set path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    fileName = file.FileName;
                }
                contact.FileName = fileName;
            }

            var result = _contactRepository.InsertContact(contact);

            return result ? "Contact saved successfully." : "Something went wrong, please try after sometime.";
        }

        public string ModifyContact(PhoneBookModel contact)
        {
            var message = string.Empty;
            if (_contactRepository.ContactExists(contact.PhoneId, contact.FirstName))
            {
                message = "Contact already exists.";
                return message;
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
                result = _contactRepository.UpdateContact(existingContact);
            }

            message = result ? "Contact updated successfully." : "Something went wrong, please try after sometime.";
            return message;
        }

        public string RemoveContact(int id)
        {
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                return "Contact deleted successfully.";
            }
            else
            {
                return "Something went wrong, please try after sometimes.";
            }
        }
    }
}
