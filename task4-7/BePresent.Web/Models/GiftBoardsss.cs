using BePresent.Domain.Users;
using System.ComponentModel.DataAnnotations;

public class GiftBoardss
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
