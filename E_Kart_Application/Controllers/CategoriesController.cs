using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(
            ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _service.GetCategoriesAsync();

            if (!data.Any())
                throw new NotFoundException("No categories found");

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var data = await _service.GetCategoryByIdAsync(id);

            if (data == null)
                throw new NotFoundException($"Category with id {id} not found");

            return Ok(data);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var data = await _service.GetProductsByCategoryAsync(id);

            if (!data.Any())
                throw new NotFoundException("No products found");

            return Ok(data);
        }

        [HttpGet("search")]
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
        public async Task<IActionResult> GetCategoriesWithProductCount()
        {
            var data = await _service.GetCategoriesWithProductCountAsync();

            if (!data.Any())
                throw new NotFoundException("No categories found");

            return Ok(data);
        }

        [HttpGet("empty")]
        public async Task<IActionResult> GetEmptyCategories()
        {
            var data = await _service.GetEmptyCategoriesAsync();

            if (!data.Any())
                throw new NotFoundException("No empty categories found");

            return Ok(data);
        }

        [HttpPost]
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