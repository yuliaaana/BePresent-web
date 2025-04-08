using BePresent.Application.DTOs;
using BePresent.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BePresent.Web.Controllers
{
    public class AuthMvcController : Controller
    {
        private readonly IUserService _userService;

        public AuthMvcController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Welcome(string username)
        {
            return View(model: username);
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userService.LoginUserAsync(dto);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(dto);
            }

            // У майбутньому тут можна зберігати у сесію або cookie
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Welcome", new { username = user.Username });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User with this email already exists");
                return View(dto);
            }

            return RedirectToAction("Login");
        }
    }
}
