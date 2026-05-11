using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerService _service;

        private readonly ILogger<CustomerApiController> _logger;

        private readonly TokenService _tokenService;

        public CustomerApiController(
            ICustomerService service,
            ILogger<CustomerApiController> logger,
            TokenService tokenService)
        {
            _service = service;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCustomers()
        {
            var data = await _service.GetCustomersAsync();
            _logger.LogInformation("Customers fetched");
            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetCustomerById(string id)
        {
            var data = await _service.GetCustomerByIdAsync(id);
            _logger.LogInformation("Customer fetched");
            return Ok(data);
        }

        [HttpGet("{id}/orders")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetCustomerOrders(string id)
        {
            var data = await _service.GetCustomerOrdersAsync(id);
            _logger.LogInformation("Customer orders fetched");
            return Ok(data);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> SearchCustomers(string name)
        {
            var data = await _service.SearchCustomersAsync(name);
            _logger.LogInformation("Customers searched");
            return Ok(data);

        }

        [HttpGet("country/{country}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCustomersByCountry(string country)
        {
            var data = await _service.GetCustomersByCountryAsync(country);
            _logger.LogInformation("Customers by country fetched");
            return Ok(data);
        }

        [HttpGet("top")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetTopCustomers()
        {
            var data = await _service.GetTopCustomersAsync();
            _logger.LogInformation("Top customers fetched");
            return Ok(data);
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterCustomerDto dto)
        {
            var data = await _service.RegisterCustomerAsync(dto);
            _logger.LogInformation("Customer registered");
            return Ok(data);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(CustomerLogin dto)
        {
            var data = await _service.LoginAsync(dto);
            if (data == null)
                throw new BadRequestException("Invalid Credentials");
            var token = _tokenService.CreateToken(data);
            _logger.LogInformation("Customer logged in");
            return Ok(new { token });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateCustomer(string id, UpdateCustomerDto dto)
        {
            var result = await _service.UpdateCustomerAsync(id, dto);
            _logger.LogInformation("Customer updated");
            return Ok("Customer updated successfully");
        }

        [HttpPatch("{id}/address")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateAddress(string id, UpdateAddressDto dto)
        {
            var result = await _service.UpdateAddressAsync(id, dto);
            _logger.LogInformation("Customer address updated");
            return Ok("Customer address updated successfully");
        }

        [HttpPatch("{id}/contact")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateContact(string id, UpdateContactDto dto)
        {
            var result = await _service.UpdateContactAsync(id, dto);
            _logger.LogInformation("Customer contact updated");
            return Ok("Customer contact updated successfully");
        }
    }
}