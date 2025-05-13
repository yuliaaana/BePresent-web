using Microsoft.EntityFrameworkCore;
using BePresent.Infrastructure.AppData;
using System;
using BePresent.Application.Interfaces;
using BePresent.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Íàëàøòóâàííÿ ï³äêëþ÷åííÿ äî áàçè äàíèõ
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Äîäàâàííÿ íåîáõ³äíèõ ñåðâ³ñ³â
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllersWithViews();
builder.Services.AddSession();


var app = builder.Build();

// Íàëàøòóâàííÿ HTTP êîíâåºðà
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AuthMvc}/{action=Login}/{id?}");



app.Run();
