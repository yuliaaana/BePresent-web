using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Додаємо простір імен для атрибутів

namespace BePresent.Application.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class UserLoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
