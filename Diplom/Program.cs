using Diplom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Diplom.Data.IdentityContext;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<SingleUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();



string? connectionString = builder.Configuration.GetConnectionString("ShafaStore");

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<IdentityContext>(options =>
        options.UseSqlServer(connectionString));
}



builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
