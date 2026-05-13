using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeService service,
            ILogger<EmployeesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var data = await _service.GetEmployeesAsync();

            _logger.LogInformation("Employees fetched");

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var data = await _service.GetEmployeeByIdAsync(id);

            if (data == null)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }

            _logger.LogInformation("Employee fetched");

            return Ok(data);
        }

        [HttpGet("managers")]
        public async Task<IActionResult> GetManagers()
        {
            var data = await _service.GetManagersAsync();

            _logger.LogInformation("Managers fetched");

            return Ok(data);
        }

        [HttpGet("reports/{id}")]
        public async Task<IActionResult> GetEmployeesUnderManager(int id)
        {
            var data = await _service.GetEmployeesUnderManagerAsync(id);

            _logger.LogInformation("Employees under manager fetched");

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(ResponseEmployeeDto dto)
        {
            var data = await _service.AddEmployeeAsync(dto);

            _logger.LogInformation("Employee created");

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee( int id, ResponseEmployeeDto dto)
        {
            var result = await _service.UpdateEmployeeAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }

            _logger.LogInformation("Employee updated");

            return Ok("Employee updated successfully");
        }

        [HttpPatch("{id}/title")]
        public async Task<IActionResult> UpdateEmployeeTitle(int id, [FromBody] string title)
        {
            var result = await _service.UpdateEmployeeTitleAsync(id, title);

            if (!result)
            {
                _logger.LogWarning("Employee not found");

                return NotFound("Employee not found");
            }

            _logger.LogInformation("Employee title updated");

            return Ok("Employee title updated successfully");
        }
        [HttpGet("{id}/territories")]
        public async Task<IActionResult> GetEmployeeTerritories(int id)
        {
            var data = await _service.GetEmployeeTerritory(id);
            return Ok(data);

        }
        [HttpPost("{id}/territories/{territoryId}")]
        public async Task<IActionResult>AddTerritoryToEmployee( int id,string territoryId)
        {
            var result = await _service.AddTerritoryToEmployeeAsync(id,territoryId);

            if (!result)
            {
                return NotFound("Employee or territory not found");
            }

            return Ok("Territory assigned successfully");
        
    }
}
}