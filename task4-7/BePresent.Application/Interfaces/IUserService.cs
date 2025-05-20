using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BePresent.Application.DTOs;
using BePresent.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BePresent.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> RegisterUserAsync(UserRegisterDto dto);
        Task<User?> LoginUserAsync(UserLoginDto dto);

        Task<User?> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

    }
}