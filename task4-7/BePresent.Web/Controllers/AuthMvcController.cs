using BePresent.Application.DTOs;
using BePresent.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BePresent.Domain.Users; // Add this for User class
using System;
using System.Threading.Tasks;

namespace BePresent.Web.Controllers
{
    public class AuthMvcController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthMvcController(
            IUserService userService,
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
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
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"Login failed: Invalid model state for email {dto.Email}");
                return View(dto);
            }

            // Пошук користувача за email
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(dto);
            }

            // Перевірка підтвердження email
            if (!user.EmailConfirmed)
            {
                Console.WriteLine($"Login failed: Email not confirmed for user {dto.Email}");
                ModelState.AddModelError(string.Empty, "Email not confirmed. Please confirm your email first.");
                return View(dto);
            }

            // Перевірка пароля
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(dto);
            }

            // Успішний вхід
            HttpContext.Session.SetInt32("UserId", user.Id);
            Console.WriteLine($"Login successful: User {user.UserName} with Id {user.Id} logged in.");

            return RedirectToAction("Welcome", new { username = user.UserName });
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"[Warning] Register: Invalid model state for user with email {dto.Email}");
                return View(dto);
            }

            var user = await _userService.RegisterUserAsync(dto);

            if (user == null)
            {
                Console.WriteLine($"[Warning] Register: Registration failed for user with email {dto.Email} - user exists or invalid password");
                ModelState.AddModelError(string.Empty, "User with this email already exists or invalid password");
                return View(dto);
            }

            Console.WriteLine($"[Info] Register: User with email {user.Email} successfully registered with Id {user.Id}");

            // Генерація токена підтвердження email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Створення посилання підтвердження email
            var confirmationLink = Url.Action("Confirm", "AuthMvc",
                new { userId = user.Id, token = token }, Request.Scheme);

            // Виведення посилання в консоль (імітація надсилання листа)
            Console.WriteLine($"[Info] Email Confirmation Link for {user.Email}: {confirmationLink}");

            // Надсилання листа (через DummyEmailSender — виведе в консоль)
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Будь ласка, підтвердіть свій email, перейшовши за посиланням: <a href='{confirmationLink}'>Confirm Email</a>");

            TempData["RegisterMessage"] = "Check your email to confirm your account.";
            return RedirectToAction("Login");
        }*/

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"[Warning] Register: Invalid model state for user with email {dto.Email}");
                return View(dto);
            }

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                // інші властивості, якщо є
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                Console.WriteLine($"[Warning] Register: Failed to register user {dto.Email}: {string.Join("; ", result.Errors)}");
                return View(dto);
            }

            // Відправка підтвердження пошти
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("Confirm", "AuthMvc",
                new { userId = user.Id, token = token }, Request.Scheme);

            Console.WriteLine($"[Info] Email Confirmation Link for {user.Email}: {confirmationLink}");

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>");

            TempData["RegisterMessage"] = "Check your email to confirm your account.";
            return RedirectToAction("Login");
        }



        [HttpGet]
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return View();
        }
    }
}