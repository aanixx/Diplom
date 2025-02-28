using Diplom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Diplom.Data.IdentityContext;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("ShafaStore");

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<IdentityContext>(options =>
        options.UseSqlServer(connectionString));
}
 

////--------------------------------------------------------------------------------////

builder.Services.AddIdentity<SingleUser, IdentityRole>(ops =>
{
    // додавання підтримки кирилиці
    ops.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+абвгдеёжзийклмнопрстуфхцчшщъыьэюяіїАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯІЇ";
    // встановлення унікальності Email
    ops.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Час життя сесії
    options.Cookie.HttpOnly = true;  // Захист від XSS
    options.Cookie.IsEssential = true; // Необхідний для роботи
});
////--------------------------------------------------------------------------------////

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
