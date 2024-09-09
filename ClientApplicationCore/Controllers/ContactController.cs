using ClientApplicationCore.Infrastructure;
using ClientApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace ClientApplicationCore.Controllers
{

    public class ContactController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IImageUpload _imageUpload;
        private string endPoint;
        public ContactController(IHttpClientService httpClientService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IImageUpload imageUpload)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _hostingEnvironment = hostingEnvironment;
            _imageUpload = imageUpload;
        }
        public IActionResult ShowAllContactWithPagination(char? letter, int page = 1, int pageSize = 2, string? searchQuery = "", string sortOrder = "asc")
        {
            
            var apiGetContactsUrl = "";
            var apiGetCountUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllContacts";
            if (letter != null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?letter={letter}&page={page}&pageSize={pageSize}&searchQuery={searchQuery}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount?letter={letter}&sortOrder={sortOrder}";
            }
            else if (!string.IsNullOrEmpty(searchQuery))
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?letter={letter}&page=" + page + "&pageSize=" + pageSize + "&searchQuery=" + searchQuery + "&sortOrder="+ sortOrder;
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount?letter={letter}&searchQuery=" + searchQuery+ "&sortOrder=" +sortOrder;
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?page=" + page + "&pageSize=" + pageSize + "&sortOrder=" + sortOrder;
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount?sortOrder="+sortOrder;
            }

            // Fetch the total count of contacts
            var countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>(apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
            if (countOfContact == null || !countOfContact.Success)
            {
                return View(new List<ContactViewModel>());
            }

            int totalCount = countOfContact.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortOrder = sortOrder;
            if (totalCount == 0)
            {
                // Return an empty view
                return View(new List<ContactViewModel>());
            }

            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("ShowAllContactWithPagination", new { letter, page = 1, pageSize, searchQuery, sortOrder });
            }


            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }
            if (response != null && response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateContactViewModel viewModel = serviceResponse.Data;
                    viewModel.Countries = GetCountry();
                    viewModel.States = GetState();
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }
        }
        [Authorize]
        [HttpPost]

        public IActionResult Edit(UpdateContactViewModel contact)
        {
            contact.Countries =GetCountry();
            contact.States = GetState();
            if (ModelState.IsValid)
            {
                if (contact.file != null && contact.file.Length > 0)
                {
                    var fileName = Path.GetFileName(contact.file.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(contact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        contact.file.CopyTo(memoryStream);
                        contact.ImageByte = memoryStream.ToArray();
                    }

                    fileName = _imageUpload.AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }
               
                if (contact.RemoveImage)
                {
                    contact.Image = string.Empty; // Set FileName to null to remove the image
                    contact.ImageByte = null;
                }
                var apiUrl = $"{endPoint}Contact/ModifyContact";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try after sometime.";
                    }
                }
            }


            return View(contact);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            AddContactViewModel viewModel = new AddContactViewModel();
            viewModel.States = GetState();
            viewModel.Countries = GetCountry();
            return View(viewModel) ;
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(AddContactViewModel contact)
        {
            contact.Countries = GetCountry();
            contact.States = GetState();
            if (ModelState.IsValid) 
            {
                if (contact.file != null && contact.file.Length > 0)
                {
                    
                    using (var memoryStream = new MemoryStream())
                    {
                        contact.file.CopyTo(memoryStream);
                        contact.ImageByte = memoryStream.ToArray();
                    }
                    var fileName = _imageUpload.AddImageFileToPath(contact.file);
                    contact.Image = fileName;
                }

                string apiUrl = $"{endPoint}Contact/Create";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, contact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse?.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;

                    }
                    else
                    {
                        TempData["ErrorMesssage"] = "Something went wrong try after some time";
                    }
                }
            }
            return View(contact);
        }

        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("ShowAllContactWithPagination");
                }
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Someething went wrong.Try again later.";
                }
                return RedirectToAction("ShowAllContactWithPagination");
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeleteConfirm(int phoneId)
        {
            var apiUrl = $"{endPoint}Contact/Remove/" + phoneId;
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("ShowAllContactWithPagination");

            }
            else
            {

                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("ShowAllContactWithPagination");

            }
        }
        

        private List<StateViewModel> GetState()
        {
            ServiceResponse<IEnumerable<StateViewModel>> response = new ServiceResponse<IEnumerable<StateViewModel>>();
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>
                ($"{endPoint}State/GetStates", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<StateViewModel>();
        }
        private List<CountryViewModel> GetCountry()
        {
            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                ($"{endPoint}Country/GetAll", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<CountryViewModel>();
        }
        public IActionResult ShowAllContactWithPaginationFav(char? letter, int page = 1, int pageSize = 2,string sortOrder="asc")
        {
            var apiGetContactsUrl = "";

            var apiGetCountUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllFavouriteContacts";
            if (letter != null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/favourites?letter={letter}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetTotalCountOfFavContacts?letter={letter}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/favourites?page=" + page + "&pageSize=" + pageSize +"&sortOrder="+ sortOrder;
                apiGetCountUrl = $"{endPoint}Contact/GetTotalCountOfFavContacts";

            }
            ServiceResponse<int> countOfContact = new ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);
            if (countOfContact == null || !countOfContact.Success)
            {
                return View(new List<ContactViewModel>());
            }
            int totalCount = countOfContact.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;

            if (totalCount == 0)
            {
                // Return an empty view
                return View(new List<ContactViewModel>());
            }


            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("ShowAllContactWithPaginationFav", new { letter, page = 1, pageSize });
            }


            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }
            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }

    }
}
