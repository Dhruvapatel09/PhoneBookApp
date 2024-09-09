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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var result = _authService.RegisterUserService(registerDto);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var response = _authService.LoginUserService(loginDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForegetDto forgetDto)
        {
            var response = _authService.ForgetPasswordService(forgetDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(string id)
        {
            var response = _authService.GetUser(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("Edit")]
        public IActionResult Edit(GetUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    userId = userDto.userId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    File = userDto.File,
                    FileName = userDto.FileName,
                    LoginId = userDto.LoginId,
                    ContactNumber = userDto.ContactNumber,
                };
                var result = _authService.ModifyUser(user);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
