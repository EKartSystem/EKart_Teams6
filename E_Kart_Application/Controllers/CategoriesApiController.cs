using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Filters;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoriesApiController> _logger;

        public CategoriesApiController(ICategoryService service,ILogger<CategoriesApiController> logger)
        {
            _service = service;
            _logger = logger;
        }

        
        [HttpGet]
        [Authorize(Roles ="Admin,Customer")]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _service.GetCategoriesAsync();

            if (!data.Any())
                throw new NotFoundException("No categories found");

            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetCategoryById(int id)
        {
            var data = await _service.GetCategoryByIdAsync(id);

            if (data == null)
                throw new NotFoundException($"Category with id {id} not found");

            return Ok(data);
        }

        [HttpGet("{id}/products")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var data = await _service.GetProductsByCategoryAsync(id);

            if (!data.Any())
                throw new NotFoundException("No products found");

            return Ok(data);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> SearchCategories([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadRequestException("Category name is required");

            var data = await _service.SearchCategoriesAsync(name);

            if (!data.Any())
                throw new NotFoundException("No matching categories found");

            return Ok(data);
        }

        [HttpGet("with-product-count")]
        [Authorize(Roles = "Admin,Customer")]

        public async Task<IActionResult> GetCategoriesWithProductCount()
        {
            var data = await _service.GetCategoriesWithProductCountAsync();

            if (!data.Any())
                throw new NotFoundException("No categories found");

            return Ok(data);
        }

        [HttpGet("empty")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetEmptyCategories()
        {
            var data = await _service.GetEmptyCategoriesAsync();

            if (!data.Any())
                throw new NotFoundException("No empty categories found");

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] ResponseCategoryDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Invalid category data");

            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                throw new BadRequestException("Category name is required");

            var data = await _service.AddCategoryAsync(dto);

            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ResponseCategoryDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Invalid category data");

            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                throw new BadRequestException("Category name is required");

            var result = await _service.UpdateCategoryAsync(id, dto);

            if (!result)
                throw new NotFoundException($"Category with id {id} not found");

            return Ok();
        }

        [HttpPatch("{id}/name")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategoryName(int id, [FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadRequestException("Category name is required");

            var result = await _service.UpdateCategoryNameAsync(id, name);

            if (!result)
                throw new NotFoundException($"Category with id {id} not found");

            return Ok();
        }

        [HttpPatch("{id}/description")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategoryDescription(int id, [FromBody] string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new BadRequestException("Description is required");

            var result = await _service.UpdateCategoryDescriptionAsync(id, description);

            if (!result)
                throw new NotFoundException($"Category with id {id} not found");

            return Ok();
        }
    }
}