using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Filters;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderApiController> _logger;

        public OrderApiController(IOrderService service, IOrderDetailService orderDetailService, ILogger<OrderApiController> logger)
        {
            _service = service;
            _orderDetailService = orderDetailService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var result = await _service.GetOrdersByCustomerAsync(customerId);
            return Ok(result);
        }

        [HttpGet("recent")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
        {
            var result = await _service.GetRecentOrdersAsync(count);
            return Ok(result);
        }

        [HttpGet("shipper/{shipperId:int}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetByShipper(int shipperId)
        {
            var result = await _service.GetOrdersByShipperAsync(shipperId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            try
            {
                var result = await _service.CreateOrderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating order for customer {CustomerId}", dto.CustomerId);
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            var result = await _service.UpdateOrderAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] DateTime? shippedDate)
        {
            var result = await _service.UpdateOrderStatusAsync(id, shippedDate);
            return Ok(result);
        }

        [HttpPatch("{id:int}/address")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateOrderAddressDto dto)
        {
            var result = await _service.UpdateOrderAddressAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/shipper")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateShipper(int id, [FromBody] int shipVia)
        {
            var result = await _service.UpdateOrderShipperAsync(id, shipVia);
            return Ok(result);
        }
    }
}