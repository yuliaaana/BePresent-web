using BePresent.Application.DTOs;
using BePresent.Application.Interfaces;
using BePresent.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BePresent.Application.Services
{

    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Просто для тесту, без реальної відправки
            Console.WriteLine($"Sending email to {email} with subject {subject}");
            return Task.CompletedTask;
        }
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /*public async Task<User?> RegisterUserAsync(UserRegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return null;

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Interests = dto.Interests
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return null;

            // Додати роль "User"
            await _userManager.AddToRoleAsync(user, "User");

            // Підтвердження email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://yourapp.com/confirm?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Click here to confirm: {confirmationLink}");

            return user;
        }*/

        public async Task<User?> RegisterUserAsync(UserRegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                Console.WriteLine($"User with email {dto.Email} already exists.");
                return null;
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Interests = dto.Interests
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                Console.WriteLine($"Failed to create user {dto.Email}. Errors:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($" - {error.Description}");
                }
                return null;
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://yourapp.com/confirm?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Click here to confirm: {confirmationLink}");

            return user;
        }


        public async Task<User?> LoginUserAsync(UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                Console.WriteLine($"LoginUserAsync: Користувач з email {dto.Email} не знайдений.");
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                Console.WriteLine($"LoginUserAsync: Невірний пароль для користувача з email {dto.Email}.");
                return null;
            }

            if (!user.EmailConfirmed)
            {
                Console.WriteLine($"LoginUserAsync: Email не підтверджено для користувача {dto.Email}.");
                return null;
            }

            Console.WriteLine($"LoginUserAsync: Успішний вхід користувача {dto.Email}.");
            return user;
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }


    }
}
