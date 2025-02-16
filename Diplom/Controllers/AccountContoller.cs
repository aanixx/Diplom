using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    public class AccountContoller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
