using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Data;
using PhoneBookApp.Models;
using PhoneBookApp.Services.Contract;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace PhoneBookApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContactService _contactService;
        public HomeController(IContactService contactService)
        {
            _contactService = contactService;
        }




        public IActionResult Index(char? letter)
        {
            if (letter.HasValue)
            {
                char lowercaseLetter = char.ToLower(letter.Value);
                var contacts = _contactService.GetContact().Where(c => c.FirstName.FirstOrDefault().ToString().ToLower() == lowercaseLetter.ToString()).ToList();
                return View(contacts);
            }
            else
            {
                var allContacts = _contactService.GetContact();
                return View(allContacts);
            }
        }
        public IActionResult Details(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
