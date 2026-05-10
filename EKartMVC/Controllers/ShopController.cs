using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductApiService _service;

        public ShopController(ProductApiService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            //var token = HttpContext.Session.GetString("JWToken");
            var prod = await _service.GetDataAsync();
            return View(model:prod);
        }
    }
}
