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

            if (data == null)
            {
                _logger.LogWarning("Customer not found for id: {id}", id);
                throw new NotFoundException ("Customer not found");
            }

            _logger.LogInformation("Customer fetched");
            return Ok(data);

        }

        [HttpGet("{id}/orders")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetCustomerOrders(string id)
        {
            var data = await _service.GetCustomerOrdersAsync(id);

            if (data == null || !data.Any())
            {
                _logger.LogWarning("No orders found for customer id: {id}", id);
                throw new NotFoundException("No orders found");
            }

            _logger.LogInformation("Customer orders fetched");
            return Ok(data);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> SearchCustomers(string name)
        {
            var data = await _service.SearchCustomersAsync(name);

            if (data == null || !data.Any())
            {
                _logger.LogWarning("No customers found for name: {name}", name);
                throw new NotFoundException ("No customers found");
            }

            _logger.LogInformation("Customers found");
            return Ok(data);

        }

        [HttpGet("country/{country}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCustomersByCountry(string country)
        {
            var data = await _service.GetCustomersByCountryAsync(country);

            if (data == null || !data.Any())
            {
                _logger.LogWarning("No customers found for country: {country}", country);
                throw new NotFoundException ("No customers found");
            }

            _logger.LogInformation("Customers by country fetched");
            return Ok(data);
        }

        [HttpGet("top")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetTopCustomers()
        {
            var data = await _service.GetTopCustomersAsync();

            if (data == null || !data.Any())
            {
                _logger.LogWarning("No top customers found");
                throw new NotFoundException("No top customers found");
            }

            _logger.LogInformation("Top customers fetched");
            return Ok(data);
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterCustomerDto dto)
        {
            var data = await _service.RegisterCustomerAsync(dto);

            if (data == null)
            {
                _logger.LogWarning("Customer registration failed");
                throw new BadRequestException("Registration failed");
            }

            _logger.LogInformation("Customer registered");
            return CreatedAtAction(nameof(Register), new { ContactName = data.ContactName }, data);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(CustomerLogin dto)
        {
            var data = await _service.LoginAsync(dto);
            if (data == null)
                throw new BadRequestException("Invalid Credentials");
            var token = _tokenService.CreateToken(data);
            _logger.LogInformation("Customer logged in");
            return Ok(new { Token = token });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateCustomer(string id, UpdateCustomerDto dto)
        {
            var result = await _service.UpdateCustomerAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Customer not found for id: {id}", id);
                return NotFound("Customer not found");
            }

            _logger.LogInformation("Customer updated");
            return Ok("Customer updated successfully");
        }

        [HttpPatch("{id}/address")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAddress(string id, [FromBody] string address)
        {
            var result = await _service.UpdateAddressAsync(id, address);

            if (!result)
                throw new NotFoundException("Customer not found");

            _logger.LogInformation("Customer address updated");
            return Ok("Customer address updated successfully");
        }
        [HttpPatch("{id}/contact")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateContact(string id, [FromBody] string contactName)
        {
            var result = await _service.UpdateContactAsync(id, contactName);

            if (!result)
                throw new NotFoundException ("Customer not found");

            _logger.LogInformation("Customer contact updated");
            return Ok("Customer contact updated successfully");
        }
    }
}