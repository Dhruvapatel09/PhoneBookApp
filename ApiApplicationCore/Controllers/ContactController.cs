using ApiApplicationCore.Dto;
using ApiApplicationCore.Models;
using ApiApplicationCore.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplicationCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService categoryService)
        {
            _contactService = categoryService;
        }
        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var response = _contactService.GetContact();
            if (!response.Success)
            {
                return NotFound(Response);
            }
            return Ok(response);
        }
        //[Authorize]
        [HttpPost("Create")]
        public IActionResult AddContact(AddContactDto contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = new PhoneBookModel()
                {
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    PhoneNumber = contactDto.PhoneNumber,
                    Company = contactDto.Company,
                    Image = contactDto.Image,
                    Gender = contactDto.Gender,
                    Favourites = contactDto.Favourites,
                    CountryId = contactDto.CountryId,
                    StateId = contactDto.StateId,
                    ImageByte = contactDto.ImageByte,
                    Birthdate = contactDto.Birthdate,

                };
                var result = _contactService.AddContact(contact);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        //[Authorize]
        [HttpPut("ModifyContact")]
        public IActionResult UpdateContact(UpdateContactDto contactDto)
        {
            var contact = new PhoneBookModel()
            {
                PhoneId = contactDto.PhoneId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                PhoneNumber = contactDto.PhoneNumber,
                Company = contactDto.Company,
                Image = contactDto.Image,
                Gender = contactDto.Gender,
                Favourites = contactDto.Favourites,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                ImageByte = contactDto.ImageByte,
                Birthdate = contactDto.Birthdate,

            };
            var response = _contactService.ModifyContact(contact);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
        //[Authorize]
        [HttpDelete("Remove/{id}")]
        public IActionResult RemoveContact(int id)
        {
            if (id > 0)
            {
                var response = _contactService.RemoveContact(id);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("Please enter proper data.");
            }
        }
        [HttpGet("GetContactById/{id}")]

        public IActionResult GetContactById(int id)
        {
            var response = _contactService.GetContact(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllContactsByPagination")]
        public IActionResult GetPaginatedContacts(char? letter, int page = 1, int pageSize = 2, string? searchQuery = "", string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            if (letter != null)
            {
                response = _contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);
            }
            else
            {
                response = _contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            }
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("GetContactsCount")]
        public IActionResult GetTotalCountOfContacts(char? letter, string? searchQuery)
        {
            var response = _contactService.TotalContacts(letter, searchQuery);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetTotalCountOfFavContacts")]
        public IActionResult GetTotalCountOfFavContacts(char? letter)
        {
            var response = _contactService.TotalFavContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("favourites")]
        public IActionResult GetFavouriteContacts(char? letter, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var response = _contactService.GetFavouriteContacts(page, pageSize, letter, sortOrder);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllFavouriteContacts")]
        public IActionResult GetAllFavouriteContacts(char? letter)
        {
            var response = _contactService.GetFavouriteContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
