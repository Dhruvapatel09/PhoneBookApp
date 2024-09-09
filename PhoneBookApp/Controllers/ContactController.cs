using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneBookApp.Data;
using PhoneBookApp.Models;
using PhoneBookApp.Services.Contract;
using PhoneBookApp.ViewModel;
using System;
using System.Diagnostics.Metrics;
using System.Security.Policy;

namespace PhoneBookApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
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



        [HttpGet]
        public IActionResult Index1()
        {

            var contact = _contactService.GetContact();
            if (contact != null && contact.Any())
            {
                return View("ContactList", contact);
            }

            return View("ContactList", new List<PhoneBookModel>());
        }

        public IActionResult Index2(int page = 1, int pageSize = 2)
        {
            var totalCount = _contactService.TotalContacts();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Validate page number
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            var contacts = _contactService.GetPaginatedContact(page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(contacts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var contact = new PhoneBookModel()
                {
                    FirstName = contactViewModel.FirstName,
                    LastName = contactViewModel.LastName,
                    Email = contactViewModel.Email,
                    PhoneNumber = contactViewModel.PhoneNumber,
                    Company = contactViewModel.Company,
                };
                var result = _contactService.AddContact(contact,contactViewModel.File);
                if (result == "Contact already exists." || result == "Something went wrong, please try after sometime.")
                {
                    TempData["ErrorMessage"] = result;
                }
                else if (result == "Contact saved successfully.")
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Index");
                }
            }

            return View(contactViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(PhoneBookModel contact)
        {
            if (ModelState.IsValid)
            {
                var message = _contactService.ModifyContact(contact);
                if (message == "Contact already exists." || message == "Something went wrong, please try after sometime.")
                {
                    TempData["ErrorMessage"] = message;
                }
                else
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }
            }

            return View(contact);
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

        public IActionResult Delete(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int phoneid)
        {
            var result = _contactService.RemoveContact(phoneid);

            if (result == "Contact deleted successfully.")
            {
                TempData["SuccessMessage"] = result;
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("Index");
        }

    }
}
