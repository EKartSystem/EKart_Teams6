using Moq;
using AutoMapper;
using E_Kart_Application.Services;
using E_Kart_Application.Repositories;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Models;
using FluentAssertions;

namespace Ekart_Test_Project
{
    public class CategoryApiTest
    {
        private readonly Mock<ICategoryRepository> _repoMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly CategoryService _service;

        public CategoryApiTest()
        {
            _repoMock =
                new Mock<ICategoryRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new CategoryService(
                    _repoMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetCategories_Positive_Test1()
        {
            var categories =
                new List<Category>
                {
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Beverages"
                    }
                };

            var dtos =
                new List<ResponseCategoryDto>
                {
                    new ResponseCategoryDto
                    {
                        CategoryId = 1,
                        CategoryName = "Beverages"
                    }
                };

            _repoMock.Setup(x =>
                x.GetCategoriesAsync())
                .ReturnsAsync(categories);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<ResponseCategoryDto>>(categories))
                .Returns(dtos);

            var result =
                await _service.GetCategoriesAsync();

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCategoryById_Positive_Test2()
        {
            var category =
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Food"
                };

            var dto =
                new ResponseCategoryDto
                {
                    CategoryId = 1,
                    CategoryName = "Food"
                };

            _repoMock.Setup(x =>
                x.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);

            _mapperMock.Setup(x =>
                x.Map<ResponseCategoryDto>(category))
                .Returns(dto);

            var result =
                await _service.GetCategoryByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddCategory_Positive_Test3()
        {
            var dto =
                new ResponseCategoryDto
                {
                    CategoryName = "Snacks",
                    Description = "Foods"
                };

            var category =
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Snacks"
                };

            var response =
                new ResponseCategoryDto
                {
                    CategoryId = 1,
                    CategoryName = "Snacks"
                };

            _mapperMock.Setup(x =>
                x.Map<Category>(dto))
                .Returns(category);

            _repoMock.Setup(x =>
                x.AddCategoryAsync(category))
                .ReturnsAsync(category);

            _mapperMock.Setup(x =>
                x.Map<ResponseCategoryDto>(category))
                .Returns(response);

            var result =
                await _service.AddCategoryAsync(dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateCategory_Positive_Test4()
        {
            var dto =
                new ResponseCategoryDto
                {
                    CategoryName = "Updated"
                };

            var category =
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Old"
                };

            _repoMock.Setup(x =>
                x.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);

            _repoMock.Setup(x =>
                x.UpdateCategoryAsync(category))
                .ReturnsAsync(true);

            var result =
                await _service.UpdateCategoryAsync(
                    1,
                    dto);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoryById_Negative_Test5()
        {
            _repoMock.Setup(x =>
                x.GetCategoryByIdAsync(100))
                .ReturnsAsync((Category?)null);

            var result =
                await _service.GetCategoryByIdAsync(100);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCategories_Negative_Test6()
        {
            var categories =
                new List<Category>();

            var dtos =
                new List<ResponseCategoryDto>();

            _repoMock.Setup(x =>
                x.GetCategoriesAsync())
                .ReturnsAsync(categories);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<ResponseCategoryDto>>(categories))
                .Returns(dtos);

            var result =
                await _service.GetCategoriesAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchCategory_Negative_Test7()
        {
            var categories =
                new List<Category>();

            var dtos =
                new List<ResponseCategoryDto>();

            _repoMock.Setup(x =>
                x.SearchCategoriesAsync("XYZ"))
                .ReturnsAsync(categories);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<ResponseCategoryDto>>(categories))
                .Returns(dtos);

            var result =
                await _service.SearchCategoriesAsync("XYZ");

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateCategory_Negative_Test8()
        {
            var dto =
                new ResponseCategoryDto
                {
                    CategoryName = "Test"
                };

            _repoMock.Setup(x =>
                x.GetCategoryByIdAsync(99))
                .ReturnsAsync((Category?)null);

            var result =
                await _service.UpdateCategoryAsync(
                    99,
                    dto);

            result.Should().BeFalse();
        }
    }
}