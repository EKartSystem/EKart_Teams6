using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService service;
        public AdminController(IAdminService _service)
        {
            service = _service;
        }

        public async Task<IActionResult> GetEmployeeall()
        {
            var data = await service.GetEmployeeAll();
            return View(data);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
