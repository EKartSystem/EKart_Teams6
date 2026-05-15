using EKartMVC.Models;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class ShipperController : Controller
    {
        private readonly ShipperApiService _service;

        public ShipperController(ShipperApiService service)
        {
            _service = service;
        }

        private string? GetToken() => HttpContext.Session.GetString("jwt");

        public async Task<IActionResult> Index()
        {
            var shippers = await _service.GetAllShippersAsync(GetToken());
            return View(shippers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var (shipper, orders) = await _service.GetShipperOrdersAsync(id, GetToken());

            if (shipper == null) return NotFound();

            ViewBag.Orders = orders;
            return View(shipper);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipperViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _service.CreateShipperAsync(model, GetToken()))
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to create shipper. Make sure you have Admin rights.");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shipper = await _service.GetShipperByIdAsync(id, GetToken());
            if (shipper == null) return NotFound();
            return View(shipper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShipperViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _service.UpdateShipperAsync(id, model, GetToken()))
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Update failed. Only Admin can update shippers.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Orders(int id)
        {
            var token = GetToken();
            var (shipper, orders) = await _service.GetShipperOrdersAsync(id, token);

            if (shipper == null) return NotFound();
            ViewBag.ShipperName = shipper.CompanyName;
            return View(orders); 
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteShipperAsync(id, GetToken());
            return RedirectToAction(nameof(Index));
        }
    }
}