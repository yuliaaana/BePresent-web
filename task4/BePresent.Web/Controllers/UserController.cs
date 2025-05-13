using Microsoft.AspNetCore.Mvc;
using BePresent.Models;
using BePresent.Infrastructure.AppData;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BePresent.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null)
                return NotFound();

            var model = new UserProfileViewModel
            {
                Username = user.Username,
                DateOfBirth = user.DateOfBirth ?? DateTime.MinValue,
                Gender = user.Gender ?? "",
                Interests = user.Interests != null ? string.Join(", ", user.Interests) : ""
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault();
            if (user == null)
                return NotFound();

            user.DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth, DateTimeKind.Utc);
            user.Gender = model.Gender;
            user.Interests = model.Interests?
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            _context.SaveChanges();

            ViewBag.SuccessMessage = "Profile updated successfully!";
            return View(model);
        }
    }
}
