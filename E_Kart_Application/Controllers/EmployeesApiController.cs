using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Filters;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeesApiController> _logger;

        public EmployeesApiController(
            IEmployeeService service,
            ILogger<EmployeesApiController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetEmployees()
        {
            var data = await _service.GetEmployeesAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var data = await _service.GetEmployeeByIdAsync(id);

            if (data == null)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }
            return Ok(data);
        }

        [HttpGet("managers")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetManagers()
        {
            var data = await _service.GetManagersAsync();
            return Ok(data);
        }

        [HttpGet("reports/{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetEmployeesUnderManager(int id)
        {
            var data = await _service.GetEmployeesUnderManagerAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee(ResponseEmployeeDto dto)
        {
            var data = await _service.AddEmployeeAsync(dto);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, ResponseEmployeeDto dto)
        {
            var result = await _service.UpdateEmployeeAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }
            return Ok(new { Message = "Employee updated successfully" });
        }

        [HttpPatch("{id}/title")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployeeTitle(int id, [FromBody] string title)
        {
            var result = await _service.UpdateEmployeeTitleAsync(id, title);

            if (!result)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }
            return Ok("Employee title updated successfully");
        }

        [HttpGet("{id}/territories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeeTerritories(int id)
        {
            var data = await _service.GetEmployeeTerritory(id);
            return Ok(data);

        }
        [HttpPost("{id}/territories/{territoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTerritoryToEmployee(int id, string territoryId)
        {
            var result = await _service.AddTerritoryToEmployeeAsync(id, territoryId);

            if (!result)
            {
                return NotFound("Employee or territory not found");
            }

            return Ok("Territory assigned successfully");

        }
    }
}