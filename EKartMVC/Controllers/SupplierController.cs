using EKartMVC.Models;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierApiService _service;

        public SupplierController(SupplierApiService service)
        {
            _service = service;
        }

        private string? GetToken() => HttpContext.Session.GetString("jwt");

        public async Task<IActionResult> Index()
        {
            var suppliers = await _service.GetAllSuppliersAsync(GetToken());
            return View(suppliers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _service.GetSupplierByIdAsync(id, GetToken());
            if (supplier == null) return NotFound();
            ViewBag.Products = await _service.GetSupplierProductsAsync(id, GetToken());
            return View(supplier);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _service.CreateSupplierAsync(model, GetToken()))
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to create supplier. Admin access required.");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _service.GetSupplierByIdAsync(id, GetToken());
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _service.UpdateSupplierAsync(id, model, GetToken()))
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update supplier.");
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _service.GetSupplierByIdAsync(id, GetToken());
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteSupplierAsync(id, GetToken());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return RedirectToAction(nameof(Index));

            var results = await _service.SearchSuppliersAsync(name, GetToken());
            ViewBag.SearchTerm = name;
            return View("Index", results);
        }
    }
}