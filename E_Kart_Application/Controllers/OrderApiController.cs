using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderApiController> _logger;

        public OrderApiController(IOrderService service, ILogger<OrderApiController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all orders");
            var result = await _service.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching order with ID {OrderId}", id);
            var result = await _service.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            _logger.LogInformation("Fetching orders for customer {CustomerId}", customerId);
            var result = await _service.GetOrdersByCustomerAsync(customerId);
            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
        {
            _logger.LogInformation("Fetching {Count} recent orders", count);
            var result = await _service.GetRecentOrdersAsync(count);
            return Ok(result);
        }

        [HttpGet("shipper/{shipperId:int}")]
        public async Task<IActionResult> GetByShipper(int shipperId)
        {
            _logger.LogInformation("Fetching orders for shipper {ShipperId}", shipperId);
            var result = await _service.GetOrdersByShipperAsync(shipperId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            try
            {
                _logger.LogInformation("Creating order for customer {CustomerId}", dto.CustomerId);
                var result = await _service.CreateOrderAsync(dto);
                _logger.LogInformation("Order created with ID {OrderId}", result.OrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating order for customer {CustomerId}", dto.CustomerId);
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            _logger.LogInformation("Updating order {OrderId}", id);
            var result = await _service.UpdateOrderAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] DateTime? shippedDate)
        {
            _logger.LogInformation("Updating status for order {OrderId}", id);
            var result = await _service.UpdateOrderStatusAsync(id, shippedDate);
            return Ok(result);
        }

        [HttpPatch("{id:int}/address")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateOrderAddressDto dto)
        {
            _logger.LogInformation("Updating address for order {OrderId}", id);
            var result = await _service.UpdateOrderAddressAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/shipper")]
        public async Task<IActionResult> UpdateShipper(int id, [FromBody] int shipVia)
        {
            _logger.LogInformation("Updating shipper for order {OrderId}", id);
            var result = await _service.UpdateOrderShipperAsync(id, shipVia);
            return Ok(result);
        }
    }
}