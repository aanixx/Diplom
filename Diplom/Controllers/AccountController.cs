using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShafaStoreDbTables;
using Diplom.Data.IdentityContext;

namespace Diplom.Controllers
{
    public class AccountController : Controller
    {
        SingleUser? user;
        UserManager<SingleUser>? _userManager;

        public AccountController(UserManager<SingleUser>? userManager)
        {
            _userManager = userManager;
        }
     
        [HttpGet]
        [Route("Account/Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Registration")]
        public async Task<IActionResult> Registration(RegViewModel? rgModel)
        {
            if (ModelState.IsValid)
            {
                if (_userManager != null && rgModel.Email != null)
                {
                    if (rgModel != null)
                    {
                        user = await _userManager.FindByEmailAsync(rgModel.Email);

                        if (user == null)
                        {
                            user = new SingleUser();
                            user.Login = rgModel.Login;
                            user.Email = rgModel.Email;

                            if (rgModel.Password != null && _userManager != null)
                            {
                                IdentityResult res = await _userManager.CreateAsync(user, rgModel.Password);
                            }
                        }
                        else
                        {
                            return RedirectToAction("UserExists", "Account");
                        }
                    }
                }
            }

            return RedirectToAction("Registration", "Account");
        }
    }
}
