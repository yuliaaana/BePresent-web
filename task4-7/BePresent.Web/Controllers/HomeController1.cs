using BePresent.Web.Models;
using BePresent.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BePresent.Web.Controllers
{
    public class Confirm : Controller
    {
        private readonly UserManager<User> _userManager;

        public Confirm(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            if (userId == 0 || string.IsNullOrEmpty(token))
            {
                return BadRequest("User ID і токен підтвердження не можуть бути порожніми.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound($"Користувача з ID {userId} не знайдено.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Content("Email успішно підтверджено. Можна перейти до входу.");
                // або return View("ConfirmEmail");
            }
            else
            {
                return Content("Помилка підтвердження емейлу.");
                // або return View("Error", new ErrorViewModel { RequestId = "Помилка підтвердження емейлу." });
            }
        }
    }
}
