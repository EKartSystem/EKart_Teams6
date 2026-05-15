using E_Kart_Application.Common;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Filters;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsApiController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var(prod, count) = await _service.GetPagedProductsAsync(pageNumber, pageSize);

            return Ok(new
            {
                Success = true,
                Data = prod,
                TotalCount = count,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int id)
        {
            var x = await _service.GetProductByIdAsync(id);
            if (x == null)
                return NotFound($"No Product Exist with Id : {id}");
            return Ok(new ApiResponse<ProductDetailsDto>
            {
                Success = true,
                Message = "Product Fetched",
                Data = x
            });
        }

        [HttpGet("in-stock")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetInStock()
        {
            var x = await _service.GetInStockProductsAsync();
            if (x == null)
                return NotFound("Products Not Found");
            return Ok(new ApiResponse<IEnumerable<ProductListingDto>>
            {
                Success = true,
                Message = "All Products in Stock are Fetched",
                Data = x
            });
        }

        [HttpGet("category/{categoryId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetProductByCategory(int categoryId)
        {
            var x = await _service.GetProductsByCategoryAsync(categoryId);
            if (x == null)
                return NotFound($"Products Not Found with CategoryId : {categoryId}");
            return Ok(new ApiResponse<IEnumerable<ProductListingDto>>
            {
                Success = true,
                Message = "All Products Fetched By CategoryId",
                Data = x
            });
        }

        [HttpGet("supplier/{supplierId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductListingDto>>> GetProductBySuppliers(int supplierId)
        {
            var x = await _service.GetProductsBySupplierAsync(supplierId);
            if (x == null)
                return NotFound($"Products Not Found with supplierId : {supplierId}");
            return Ok(new ApiResponse<IEnumerable<ProductListingDto>>
            {
                Success = true,
                Message = "All Products Fetched by SupplierId",
                Data = x
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDetailsDto>> Create(ProductDto createDto)
        {
            var result = await _service.AddProductAsync(createDto);
            if (result == null)
                return BadRequest("Not able to Add Product. Something went wrong");
            return CreatedAtAction(nameof(GetProductById), new { id = result.ProductId },
                new ApiResponse<ProductDetailsDto>
                 {
                     Success = true,
                     Message = "Product Created Successfully",
                     Data = result
                 });
            //basically createdAtAction => nameof is used to view this item in future the new{id} is provided
            //and the result is the json format.
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ProductDto updateDto)
        {
            if (updateDto == null) 
                return BadRequest("Update data is required.");
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
            if (results == null)
                return NotFound($"Products Not Found with name : {name}");
            return Ok(new ApiResponse<IEnumerable<ProductListingDto>>
            {
                Success = true,
                Message = $"All Products Fetched with Name {name}",
                Data = results
            });
        }

        [HttpGet("expensiveProducts")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<IEnumerable<ProductDetailsDto>>> TenExpensiveProduct()
        {
            var x = await _service.GetExpensiveProductsAsync();
            if (x == null)
                return NotFound("No Expensive Products Found");
            return Ok(new ApiResponse<IEnumerable<ProductDetailsDto>>
            {
                Success = true,
                Message = "TOP 10 Expensive Products",
                Data = x
            });
        }
    }
}
