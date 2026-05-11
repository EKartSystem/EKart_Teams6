using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService service,ILogger<CategoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _service.GetCategoriesAsync();

            _logger.LogInformation("Categories fetched");

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var data = await _service.GetCategoryByIdAsync(id);

            if (data == null)
            {
                _logger.LogWarning("Category not found");

                return NotFound("Category not found");
            }

            _logger.LogInformation("Category fetched");

            return Ok(data);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var data = await _service.GetProductsByCategoryAsync(id);

            _logger.LogInformation("Products fetched");

            return Ok(data);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategories(string name)
        {
            var data = await _service.SearchCategoriesAsync(name);

            _logger.LogInformation("Categories searched");

            return Ok(data);
        }

        [HttpGet("with-product-count")]
        public async Task<IActionResult> GetCategoriesWithProductCount()
        {
            var data = await _service.GetCategoriesWithProductCountAsync();

            _logger.LogInformation("Categories with product count fetched");

            return Ok(data);
        }

        [HttpGet("empty")]
        public async Task<IActionResult> GetEmptyCategories()
        {
            var data = await _service.GetEmptyCategoriesAsync();

            _logger.LogInformation("Empty categories fetched");

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDto dto)
        {
            var data = await _service.AddCategoryAsync(dto);

            _logger.LogInformation("Category created");

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            var result = await _service.UpdateCategoryAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Category not found");

                return NotFound("Category not found");
            }

            _logger.LogInformation("Category updated");

            return Ok("Category updated successfully");
        }

        [HttpPatch("{id}/name")]
        public async Task<IActionResult> UpdateCategoryName(int id, UpdateCategoryNameDto dto)
        {
            var result = await _service.UpdateCategoryNameAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Category not found");

                return NotFound("Category not found");
            }

            _logger.LogInformation("Category name updated");

            return Ok("Category name updated successfully");
        }

        [HttpPatch("{id}/description")]
        public async Task<IActionResult> UpdateCategoryDescription(int id, UpdateCategoryDescriptionDto dto)
        {
            var result = await _service.UpdateCategoryDescriptionAsync(id, dto);

            if (!result)
            {
                _logger.LogWarning("Category not found");

                return NotFound("Category not found");
            }

            _logger.LogInformation("Category description updated");

            return Ok("Category description updated successfully");
        }
    }
}