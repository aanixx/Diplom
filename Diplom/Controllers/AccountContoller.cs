using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShafaStoreDbTables;
using Diplom.Data.IdentityContext;

namespace Diplom.Controllers
{
    public class AccountContoller : Controller
    {
        SingleUser? user;
        UserManager<SingleUser>? _userManager;

        public AccountContoller(UserManager<SingleUser>? userManager)
        {
            _userManager = userManager;
        }
     
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegViewModel? rgModel)
        {
            if (ModelState.IsValid)
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

            return RedirectToAction("Registration", "Account");
        }
    }
}
