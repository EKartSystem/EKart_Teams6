using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationApiController : ControllerBase
    {
        private readonly ILocationService _service;

        public LocationApiController(ILocationService service)
        {
            _service = service;
        }

        [HttpGet("regions")]
        [Authorize(Roles ="Admin,Customer")]
        public async Task<ActionResult<List<RegionDto>>> GetRegions()
        {
            var regions = await _service.GetAllRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("regions/{id}")]
        [Authorize(Roles ="Admin,Customer")]
        public async Task<ActionResult<RegionDto>> GetRegion(int id)
        {
            var region = await _service.GetRegionByIdAsync(id);
            return Ok(region);
        }

        [HttpGet("territories")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<List<TerritoryDto>>> GetTerritories()
        {
            var territories = await _service.GetAllTerritoriesAsync();
            return Ok(territories);
        }

        [HttpGet("regions/{regionId}/territories")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<List<TerritoryDto>>> GetTerritoriesByRegion(int regionId)
        {
            var territories = await _service.GetTerritoriesByRegionIdAsync(regionId);
            return Ok(territories);
        }

        [HttpGet("territories/{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<TerritoryDto>> GetTerritory(string id)
        {
            var territory = await _service.GetTerritoryByIdAsync(id);
            return Ok(territory);
        }

        [HttpPost("territories")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TerritoryDto>> CreateTerritory([FromBody] TerritoryDto territoryDto)
        {
            var created = await _service.CreateTerritoryAsync(territoryDto);
            return CreatedAtAction(nameof(GetTerritory), new { id = created.TerritoryId }, created);
        }

        [HttpPut("territories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTerritory(string id, [FromBody] TerritoryDto territoryDto)
        {
            if (id != territoryDto.TerritoryId)
            {
                return BadRequest("Id does not matchc");
            }
            await _service.UpdateTerritoryAsync(id, territoryDto);
            return NoContent(); 
        }
    }
}
