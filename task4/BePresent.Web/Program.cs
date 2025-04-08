using Serilog;
using Microsoft.EntityFrameworkCore;
using BePresent.Infrastructure.AppData;
using BePresent.Application.Interfaces;
using BePresent.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ======= Serilog Configuration =======
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341") // стандартна адреса Seq
    .CreateLogger();

builder.Host.UseSerilog(); // <-- Важливо: інтеграція з хостом

// ======= Add Services and DB Context =======
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ======= Configure Middleware =======
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ======= Routing =======
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AuthMvc}/{action=Login}/{id?}");

// ======= Run =======
app.Run();
