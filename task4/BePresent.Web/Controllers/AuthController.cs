/*using BePresent.Application.DTOs;
using BePresent.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BePresent.Domain.Users;
using BePresent.Application.Services;



namespace BePresent.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _userService.LoginAsync(username, password);
            if (result)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            var result = await _userService.RegisterAsync(username, email, password);
            if (result)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Registration failed.");
            return View();
        }
    }
}



/*
namespace BePresent.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);
            if (user == null)
                return BadRequest("User already exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userService.LoginUserAsync(dto);
            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }
    }
}*/

//using BePresent.Application.Users;