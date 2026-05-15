using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Models;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationApiService _service;

        public LocationController(LocationApiService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? regionId) 
        {
            var token = HttpContext.Session.GetString("jwt");
            var regions = await _service.GetAllRegionsAsync(token);

            if (regionId.HasValue)
            {
                var territories = await _service.GetTerritoriesByRegionAsync(regionId.Value, token);
                ViewBag.Territories = territories;
                ViewBag.SelectedRegionId = regionId.Value;
            }
            return View(regions);
        }
        [HttpGet("Location/AllRegions")]
        public async Task<IActionResult> GetRegions()
        {
            var token = HttpContext.Session.GetString("jwt");
            var regions = await _service.GetAllRegionsAsync(token);
            return Json(regions);
        }
        public async Task<IActionResult> Territories(int id)
        {
            var token = HttpContext.Session.GetString("jwt");
            var territories = await _service.GetTerritoriesByRegionAsync(id, token);
            ViewBag.RegionId = id;
            return View(territories);
        }
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("jwt");
            ViewBag.Regions = await _service.GetAllRegionsAsync(token);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TerritoryDto dto)
        {
            var token = HttpContext.Session.GetString("jwt");
            if (await _service.CreateTerritoryAsync(dto, token))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Regions = await _service.GetAllRegionsAsync(token);
            return View(dto);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var token = HttpContext.Session.GetString("jwt");
            var territory = await _service.GetTerritoryByIdAsync(id, token);
            if (territory == null) return NotFound();

            ViewBag.Regions = await _service.GetAllRegionsAsync(token);
            return View(territory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, TerritoryDto dto)
        {
            var token = HttpContext.Session.GetString("jwt");
            if (await _service.UpdateTerritoryAsync(id, dto, token))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Regions = await _service.GetAllRegionsAsync(token);
            return View(dto);
        }
    }
}