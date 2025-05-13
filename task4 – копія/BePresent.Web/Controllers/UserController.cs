using BePresent.Domain.Users;
using BePresent.Infrastructure.AppData;
using Microsoft.AspNetCore.Mvc;

public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // Дія для відображення списку користувачів
    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    // Дія для створення нового користувача
    public IActionResult Create()
    {
        return View();
    }

    // Дія для створення нового користувача (збереження)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }
}
