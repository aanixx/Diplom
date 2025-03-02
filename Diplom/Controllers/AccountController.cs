using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Diplom.Data.IdentityContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Diplom.Controllers
{
    public class AccountController : Controller
    {
        UserManager<SingleUser>? _userManager;
        SignInManager<SingleUser>? _signInManager;
        SingleUser? user;

        public AccountController(UserManager<SingleUser>? userManager, SignInManager<SingleUser>? signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("Account/Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Registration")]
        public async Task<IActionResult> Registration(RegViewModel rgModel)
        {
            if (!ModelState.IsValid)
            {
                return View(rgModel);
            }

            if (_userManager == null || _signInManager == null)
            {
                ModelState.AddModelError("", "Authentication service is unavailable.");
                return View(rgModel);
            }

            var existingUser = await _userManager.FindByEmailAsync(rgModel.Email);
            if (existingUser != null)
            {
                return RedirectToAction("UserExists", "Account");
            }

            var user = new SingleUser
            {
                UserName = rgModel.UserName,
                Login = rgModel.Login,
                Email = rgModel.Email,
                PhoneNumber = rgModel.PhoneNumber
            };

            // Создаем пользователя (пароль хешируется автоматически)
            var res = await _userManager.CreateAsync(user, rgModel.Password);
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(rgModel);
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel? login)
        {
            if (ModelState.IsValid)
            {
                if (_signInManager != null && login != null)
                {
                    var res = await _signInManager.PasswordSignInAsync(login.UserName!, login.Password!, login.RememberMe, false);
                    if (res.Succeeded)
                    {
                        // Отримуємо користувача з БД
                        var user = await _userManager.FindByNameAsync(login.UserName!);
                        if (user != null)
                        {
                            // Зберігаємо дані в сесії
                            HttpContext.Session.SetString("UserId", user.Id);
                            HttpContext.Session.SetString("Username", user.UserName!);
                        }

                        if (login.ReturnUrl != null && Url.IsLocalUrl(login.ReturnUrl))
                        {
                            return Redirect(login.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Wrong login or password");
            }



            return View(login);
        }

        // Метод виходу
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager != null)
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear(); // Очищення всієї сесії
            }

            return RedirectToAction("Index", "Home");
        }

    }

}
