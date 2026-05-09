using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Filters;
using E_Kart_Application.Models;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsApiController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles ="Admin,Customer")]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetALlProducts()
        {
            var x = await _service.GetAllProductsAsync();
            return Ok(x);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int id)
        {
            var x = await _service.GetProductByIdAsync(id);
            return Ok(x);
        }

        [HttpGet("in-stock")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetInStock()
        {
            var products = await _service.GetInStockProductsAsync();
            return Ok(products);
        }

        [HttpGet("category/{categoryId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetProductByCategory(int categoryId)
        {
            var x = await _service.GetProductsByCategoryAsync(categoryId);
            return Ok(x);
        }

        [HttpGet("supplier/{supplierId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetProductBySuppliers(int supplierId)
        {
            var x = await _service.GetProductsBySupplierAsync(supplierId);
            return Ok(x);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDetailsDto>> Create(CreateProductDto createDto)
        {
            var result = await _service.AddProductAsync(createDto);
            return CreatedAtAction(nameof(GetProductById), new { id = result.ProductId }, result);
            //basically createdAtAction => nameof is used to view this item in future the new{id} is provided
            //and the result is the json format.
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateProductDto updateDto)
        {
            await _service.UpdateProductAsync(id, updateDto);
            return NoContent();
        }

        [HttpPatch("{id}/price")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] decimal newPrice)
        {
            await _service.UpdatePriceAsync(id, newPrice);
            return Ok(new { Message = "Price updated successfully" });
        }

        [HttpPatch("{id}/stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] short units)
        {
            await _service.UpdateStockAsync(id, units);
            return Ok(new { Message = "Stock updated successfully" });
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> SearchProductsByName([FromQuery] string name)
        {
            var results = await _service.SearchProductsAsync(name);
            return Ok(results);
        }

        [HttpGet("expensiveProducts")]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<IEnumerable<ExpensiveProductDto>>> TenExpensiveProduct()
        {
            var x = await _service.GetExpensiveProductsAsync();
            return Ok(x);
        }

    }
}
