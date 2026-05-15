using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.Filters;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _service;
        public OrderDetailsController(IOrderDetailService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetAll()
        {
            var orderDetails = await _service.GetAllAsync();
            return Ok(orderDetails);
        }

        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var orderDetails = await _service.GetByOrderIdAsync(orderId);
            return Ok(orderDetails);
        }

        [HttpGet("{orderId}/{productId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetById(int orderId, int productId)
        {
            var orderDetail = await _service.GetByIdAsync(orderId, productId);
            return Ok(orderDetail);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateOrderDetailDto dto)
        {
            var createdOrderDetail = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    orderId = createdOrderDetail.OrderId,
                    productId = createdOrderDetail.ProductId
                },
                createdOrderDetail
            );
        }

        [HttpPut("{orderId}/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int orderId, int productId, UpdateOrderDetailDto dto)
        {
            var updatedOrderDetail = await _service.UpdateAsync(orderId, productId, dto);
            return Ok(updatedOrderDetail);
        }

        [HttpDelete("{orderId}/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int orderId, int productId)
        {
            await _service.DeleteAsync(orderId, productId);
            return NoContent();
        }
    }
}