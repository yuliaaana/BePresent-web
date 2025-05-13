using BePresent.Domain.Users;
using System;
public class BoardsViewModel
{
    public GiftBoard Board { get; set; } = default!;
    public List<Gift> Gifts { get; set; } = new();
}
