using ApiApplicationCore.Dto;
using ApiApplicationCore.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplicationCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IContactService _messageService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IContactService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }
        //[HttpGet("TotalContacts")]
        //public IActionResult TotalContacts()
        //{
        //    var response = _messageService.TotalContacts();
        //    if (!response.Success)
        //    {
        //        return NotFound(response);
        //    }
        //    return Ok(response);

        //}


    }
}
