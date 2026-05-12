using E_Kart_Application.Controllers;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace E_Kart_Application.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _serviceMock;
        private readonly Mock<ILogger<CategoriesApiController>> _loggerMock;
        private readonly CategoriesApiController _controller;

        public CategoriesControllerTests()
        {
            _serviceMock = new Mock<ICategoryService>();
            _loggerMock = new Mock<ILogger<CategoriesApiController>>();

            _controller = new CategoriesApiController(
                _serviceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetCategories_Positive()
        {
            var categories = new List<CategoryDto>
            {
                new CategoryDto
                {
                    CategoryId = 1,
                    CategoryName = "Beverages",
                    Description = "Drinks"
                }
            };

            _serviceMock
                .Setup(x => x.GetCategoriesAsync())
                .ReturnsAsync(categories);

            var result = await _controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(categories, okResult.Value);
        }

        [Fact]
        public async Task GetCategoryById_Positive()
        {
            var category = new CategoryDto
            {
                CategoryId = 1,
                CategoryName = "Beverages",
                Description = "Drinks"
            };

            _serviceMock
                .Setup(x => x.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);

            var result = await _controller.GetCategoryById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task AddCategory()
        {
            var dto = new CreateCategoryDto
            {
                CategoryName = "Snacks",
                Description = "Food"
            };

            var resultDto = new CategoryDto
            {
                CategoryId = 2,
                CategoryName = "Snacks",
                Description = "Food"
            };

            _serviceMock
                .Setup(x => x.AddCategoryAsync(dto))
                .ReturnsAsync(resultDto);

            var result = await _controller.AddCategory(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(resultDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateCategory()
        {
            var dto = new UpdateCategoryDto
            {
                CategoryName = "Updated",
                Description = "Updated Desc"
            };

            _serviceMock
                .Setup(x => x.UpdateCategoryAsync(1, dto))
                .ReturnsAsync(true);

            var result = await _controller.UpdateCategory(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Category updated successfully", okResult.Value);
        }

        [Fact]
        public async Task GetCategoryById()
        {
            _serviceMock
                .Setup(x => x.GetCategoryByIdAsync(99))
                .ReturnsAsync((CategoryDto?)null);

            var result = await _controller.GetCategoryById(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCategory2()
        {
            var dto = new UpdateCategoryDto
            {
                CategoryName = "Invalid",
                Description = "Invalid"
            };

            _serviceMock
                .Setup(x => x.UpdateCategoryAsync(99, dto))
                .ReturnsAsync(false);

            var result = await _controller.UpdateCategory(99, dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCategoryName()
        {
            var dto = new UpdateCategoryNameDto
            {
                CategoryName = "New Name"
            };

            _serviceMock
                .Setup(x => x.UpdateCategoryNameAsync(99, dto))
                .ReturnsAsync(false);

            var result = await _controller.UpdateCategoryName(99, dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCategoryDescription()
        {
            var dto = new UpdateCategoryDescriptionDto
            {
                Description = "New Desc"
            };

            _serviceMock
                .Setup(x => x.UpdateCategoryDescriptionAsync(99, dto))
                .ReturnsAsync(false);

            var result = await _controller.UpdateCategoryDescription(99, dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }
    }
}