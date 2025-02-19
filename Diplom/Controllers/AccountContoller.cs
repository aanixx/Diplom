using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    public class AccountContoller : Controller
    {


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
    }
}
