using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Додаємо простір імен для атрибутів

namespace BePresent.Domain.Users
{
    public class User
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public string? Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<string>? Interests { get; set; }
        public bool IsAuthorized { get; set; }

        public ICollection<GiftBoard> GiftBoards { get; set; } = new List<GiftBoard>();
        public ICollection<GiftReservation> GiftReservations { get; set; } = new List<GiftReservation>();
        public ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();
    }

    public class GiftBoard
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int BoardId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime? CelebrationDate { get; set; }
        public List<int>? Collaborators { get; set; }
        public string AccessLevel { get; set; } = "private";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }

    public class Gift
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int GiftId { get; set; }
        public int BoardId { get; set; }
        public GiftBoard Board { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsReserved { get; set; } = false;
        public int? ReservedBy { get; set; }
        public User? ReservedUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class GiftReservation
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int ReservationId { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; } = default!;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
    }

    public class ActionLog
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string Action { get; set; } = default!;
        public DateTime ActionTime { get; set; } = DateTime.UtcNow;
    }

    public class EmailConfirmation
    {
        [Key]  // Вказуємо, що це первинний ключ
        public int ConfirmationId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public string ConfirmationToken { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
    }
}
