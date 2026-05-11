using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
