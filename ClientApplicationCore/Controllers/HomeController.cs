using ClientApplicationCore.Infrastructure;
using ClientApplicationCore.Models;
using ClientApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ClientApplicationCore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        public HomeController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }
        public IActionResult Index()
        {
            //ServiceResponse<int> response = new ServiceResponse<int>();
            //response = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
            //    ($"{endPoint}Home/TotalContacts", HttpMethod.Get, HttpContext.Request);


            //if (response.Success)
            //{
            //    return View(response.Data);
            //}
            return View();
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
