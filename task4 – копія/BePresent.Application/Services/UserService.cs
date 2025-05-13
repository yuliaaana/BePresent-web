using BePresent.Application.DTOs;
using BePresent.Application.Interfaces;
using BePresent.Domain.Users;
using BePresent.Infrastructure.AppData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace BePresent.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> RegisterUserAsync(UserRegisterDto dto)
        {
            try
            {
                Log.Information("Attempting to register user with email: {Email}", dto.Email);

                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                {
                    Log.Warning("Registration failed: Email {Email} already exists", dto.Email);
                    return null;
                }

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = HashPassword(dto.Password),
                    IsAuthorized = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Log.Information("User registered successfully with email: {Email}", dto.Email);
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during user registration for email: {Email}", dto.Email);
                throw;
            }
        }

        public async Task<User?> LoginUserAsync(UserLoginDto dto)
        {
            try
            {
                Log.Information("Attempting to login user with email: {Email}", dto.Email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null || user.Password != HashPassword(dto.Password))
                {
                    Log.Warning("Login failed for email: {Email}", dto.Email);
                    return null;
                }

                user.IsAuthorized = true;
                await _context.SaveChangesAsync();

                Log.Information("User logged in successfully with email: {Email}", dto.Email);
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during user login for email: {Email}", dto.Email);
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
