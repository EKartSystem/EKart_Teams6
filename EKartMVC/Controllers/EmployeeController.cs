using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AdminServices service;

        public EmployeeController(AdminServices _service)
        {
            service = _service;
        }
        private string? GetToken() => HttpContext.Session.GetString("jwt");

        public async Task<IActionResult> GetEmployeeall()
        {
            var data = await service.GetEmployeeAll(GetToken());
            return View(data);
        }

        public async Task<IActionResult> GetEmployeeid(int id)
        {
            var data = await service.GetEmployeebyId(id, GetToken());
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ResponseEmployeeDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResponseEmployeeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await service.Addemployee(dto, GetToken());

            if (!result)
            {
                ModelState.AddModelError("", "Employee not saved. Ensure you have Admin privileges.");
                return View(dto);
            }

            return RedirectToAction(nameof(GetEmployeeall));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await service.GetEmployeebyId(id, GetToken());
            if (data == null) return NotFound();

            ViewBag.EmployeeId = id;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResponseEmployeeDto dto)
        {
            if (!string.IsNullOrEmpty(dto.HomePhone))
            {
                dto.HomePhone= new string(dto.HomePhone.Where(char.IsDigit).ToArray());
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeId = id;
                return View(dto);
            }
            var result = await service.Updateemployee(id, dto, GetToken());

            if (!result)
            {
                ModelState.AddModelError("", "Update failed. Verify the Manager ID exists and phone is 10 digits.");
                ViewBag.EmployeeId = id;
                return View(dto);
            }

            return RedirectToAction(nameof(GetEmployeeall));
        }

        public async Task<IActionResult> Getmanager()
        {
            var data = await service.GetManagers(GetToken());
            return View(data);
        }

        public async Task<IActionResult> Getemployeemanager(int id)
        {
            var data = await service.GetEmployeeManagers(id, GetToken());
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Updatetitle(int id)
        {
            var data = await service.GetEmployeebyId(id, GetToken());
            ViewBag.EmployeeId = id;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Updatetitle(int id, string title)
        {
            var result = await service.Updateemployeetitle(id, title, GetToken());

            if (!result)
            {
                ModelState.AddModelError("", "Title update failed");
                var data = await service.GetEmployeebyId(id, GetToken());
                ViewBag.EmployeeId = id;
                return View(data);
            }

            return RedirectToAction(nameof(GetEmployeeall));
        }

        public async Task<IActionResult> GetCategories()
        {
            var data = await service.GetCategories(GetToken());
            return View(data ?? new List<ResponseCategoryDto>());
        }

        public async Task<IActionResult> GetCategoryById(int id)
        {
            var data = await service.GetCategoryById(id, GetToken());
            return View(data);
        }

        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var data = await service.GetProductsByCategory(id, GetToken());
            return View(data ?? new List<E_Kart_Application.DTOs.ProductsDTO.ProductListingDto>());
        }

        public async Task<IActionResult> GetCategoriesWithProductCount()
        {
            var data = await service.GetCategoriesWithProductCount(GetToken());
            return View(data ?? new List<E_Kart_Application.DTOs.CategoryDto.CategoryDto>());
        }

        public async Task<IActionResult> GetEmptyCategories()
        {
            var data = await service.GetEmptyCategories(GetToken());
            return View(data ?? new List<ResponseCategoryDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(ResponseCategoryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await service.AddCategory(dto, GetToken());
            return RedirectToAction(nameof(GetCategories));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var data = await service.GetCategoryById(id, GetToken());
            if (data == null) return NotFound();
            ViewBag.CategoryId = id;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, ResponseCategoryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await service.UpdateCategory(id, dto, GetToken());
            return RedirectToAction(nameof(GetCategories));
        }

        public async Task<IActionResult> SearchCategory(string name)
        {
            var data = await service.SearchCategories(name, GetToken());
            return View(data ?? new List<ResponseCategoryDto>());
        }

        public async Task<IActionResult> GetTerritory(int id)
        {
            var territory = await service.GetTerritoriesAsync(id, GetToken());
            ViewBag.EmployeeId = id;
            return View(territory);
        }

        [HttpPost]
        public async Task<IActionResult> AddTerritory(int id, TerritoryDto dto, int territoryId)
        {
            var result = await service.Addterritories(id, dto, territoryId, GetToken());

            if (!result)
            {
                ModelState.AddModelError("", "Territory not added");
                return View(dto);
            }

            return RedirectToAction(nameof(GetEmployeeall));
        }
    }
}