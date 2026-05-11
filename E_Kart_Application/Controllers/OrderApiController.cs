using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderApiController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var result = await _service.GetOrdersByCustomerAsync(customerId);
            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
        {
            var result = await _service.GetRecentOrdersAsync(count);
            return Ok(result);
        }

        [HttpGet("shipper/{shipperId:int}")]
        public async Task<IActionResult> GetByShipper(int shipperId)
        {
            var result = await _service.GetOrdersByShipperAsync(shipperId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var result = await _service.CreateOrderAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            var result = await _service.UpdateOrderAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            var result = await _service.UpdateOrderStatusAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/address")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateOrderAddressDto dto)
        {
            var result = await _service.UpdateOrderAddressAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/shipper")]
        public async Task<IActionResult> UpdateShipper(int id, UpdateOrderShipperDto dto)
        {
            var result = await _service.UpdateOrderShipperAsync(id, dto);
            return Ok(result);
        }
    }
}