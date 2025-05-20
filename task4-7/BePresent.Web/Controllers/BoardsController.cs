using BePresent.Domain.Users;
using BePresent.Infrastructure.AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BePresent.Domain.Users;


namespace BePresent.Controllers
{
    public class BoardsController : BaseController
    {
        private readonly AppDbContext _context;

        public BoardsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Отримуємо список всіх дошок
            var giftBoards = await _context.GiftBoards.ToListAsync();
            return View(giftBoards); 
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiftBoard newBoard)
        {
            // Виведення для дебагу
            Console.WriteLine("Creating new board");

            // Отримання ID поточного користувача
            int currentUserId = GetUserId() ?? 0; // Ваша логіка отримання ID користувача
            newBoard.UserId = currentUserId;

            // Перевірка, чи є дата і перетворення в UTC, якщо це необхідно
            if (newBoard.CelebrationDate.HasValue)
            {
                if (newBoard.CelebrationDate.Value.Kind != DateTimeKind.Utc)
                {
                    newBoard.CelebrationDate = newBoard.CelebrationDate.Value.ToUniversalTime();
                }
            }

            // Перевірка, чи є помилки в моделі
            if (!ModelState.IsValid)
            {
                // Виведення всіх помилок у консоль для дебагу
                foreach (var kvp in ModelState)
                {
                    var key = kvp.Key;
                    var errors = kvp.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Model error in '{key}': {error.ErrorMessage}");
                    }
                }

                // Повертаємо до форми з помилками
                return View(newBoard);
            }

            // Якщо модель коректна, додаємо нову дошку і зберігаємо в базу
            _context.GiftBoards.Add(newBoard);
            await _context.SaveChangesAsync();

            // Перенаправляємо на головну сторінку після успішного збереження
            return RedirectToAction(nameof(Index));
        }



    }
}


