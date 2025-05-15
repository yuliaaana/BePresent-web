using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Додаємо простір імен для атрибутів

namespace BePresent.Application.DTOs
{
    public class UserRegisterDto
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<string>? Interests { get; set; }
    }


    public class UserLoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
