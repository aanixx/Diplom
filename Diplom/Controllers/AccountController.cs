using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Diplom.Data.IdentityContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [HttpGet]
        [Route("Account/Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
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
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string? returnUrl = Url.Action("GoogleResponse", "Account", new { returnUrl = "/" });
            var properties = _signInManager?.ConfigureExternalAuthenticationProperties("Google", returnUrl);
            if (properties == null)
            {
                Console.WriteLine("Properties are null");
            }
            Console.WriteLine("Redirecting to Google with returnUrl: " + returnUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        [Route("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = "/")
        {
            if (_signInManager == null || _userManager == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();
            Console.WriteLine($"ExternalLoginInfo: {info}");
            if (info == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Проверяем, есть ли пользователь с таким логином
            var resLogin = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (resLogin.Succeeded)
            {
                return LocalRedirect(returnUrl!);
            }

            // Создаем пользователя, если его нет
            string? email = info.Principal.FindFirstValue(ClaimTypes.Email);
            string? userName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? info.Principal.FindFirstValue(ClaimTypes.GivenName);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Email or username is null or empty");
                return RedirectToAction("AccessDenied", "Account");
            }

            SingleUser? user = await _userManager.FindByEmailAsync(email);
            IdentityResult? result = null;

            if (user == null)
            {
                user = new SingleUser
                {
                    UserName = userName,
                    Email = email
                };

                result = await _userManager.CreateAsync(user);
            }

            if (result == null || result.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl!);
            }

            return RedirectToAction("AccessDenied", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Login = user.Login,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // Изменяем через UserManager там, где требуется
                var emailResult = await _userManager.SetEmailAsync(user, model.Email);
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
        

                // Если все изменения успешно прошли
                if (emailResult.Succeeded && phoneResult.Succeeded)
                {
                    user.Login = model.Login;
                    user.UserName = model.UserName;
                    var updateResult = await _userManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return RedirectToAction("Profile");
                    }
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    // Собираем ошибки с всех операций
                    var allResults = new[] { emailResult, phoneResult };
                    foreach (var result in allResults)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        [Route("Account/UserExists")]
        public IActionResult UserExists()
        {
            return View();
        }

    }

}
