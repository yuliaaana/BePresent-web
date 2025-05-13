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

    // ĳ� ��� ����������� ������ ������������
    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    // ĳ� ��� ��������� ������ �����������
    public IActionResult Create()
    {
        return View();
    }

    // ĳ� ��� ��������� ������ ����������� (����������)
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
