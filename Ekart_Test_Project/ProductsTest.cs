using AutoMapper;
using E_Kart_Application.Controllers;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ekart_Test_Project
{
    public class ProductsTest
    {
        [Fact] 
        public async Task GetTenMostExpensiveProductsTest()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var FakeProducts = new List<ExpensiveProductDto>
            {
                new ExpensiveProductDto { TenMostExpensiveProducts = "Product A", UnitPrice = 100 },
                new ExpensiveProductDto { TenMostExpensiveProducts = "Product B", UnitPrice = 90 }
            };
            mockRepo.Setup(x => x.GetExpensiveProductsAsync()).ReturnsAsync(FakeProducts);
            var service = new ProductService(mockRepo.Object,mockMapper.Object);
            var result = await service.GetExpensiveProductsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product A", result.First().TenMostExpensiveProducts);
            mockRepo.Verify(x => x.GetExpensiveProductsAsync(), Times.Once);
        }
        [Fact] 
        public async Task GetTenMostExpensiveProductsTest1()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(x => x.GetExpensiveProductsAsync()).ReturnsAsync(null as List<ExpensiveProductDto>);
            var mockService = new ProductService(mockRepo.Object, mockMapper.Object);
            Assert.ThrowsAsync<NotFoundException>(() => mockService.GetExpensiveProductsAsync());
            mockRepo.Verify(x => x.GetExpensiveProductsAsync(), Times.Once);
        }

        [Fact] 
        public async Task GetProductByIdTest()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var p = new Product()
            {
                ProductId = 1234,
                UnitPrice = 5000
            };
            var dto = new ProductDetailsDto()
            {
                ProductId = 1234,
                UnitPrice = 5000
            };
            mockRepo.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(p);
            mockMapper.Setup(x =>x.Map<ProductDetailsDto>(p)).Returns(dto);

            var mockService = new ProductService(mockRepo.Object, mockMapper.Object);
            var prod = await mockService.GetProductByIdAsync(1);

            Assert.NotNull(prod);
            Assert.Equal(1234, prod.ProductId);
        }

        [Fact] 
        public async Task GetProductByIdTest1()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(null as Product);

            var service =new ProductService(mockRepo.Object,mockMapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                service.GetProductByIdAsync(1));
        }

        [Fact]

        public async Task GetProductsBySearch()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var p = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    UnitPrice = 200
                }
            };
            var x = new List<ProductListingDto>
            {
                new ProductListingDto()
                {
                    ProductId = 1,
                    UnitPrice = 200
                }
            };

            mockRepo.Setup(x => x.SearchProductsByNameAsync("chai")).ReturnsAsync(p);
            mockMapper.Setup(m => m.Map<IEnumerable<ProductListingDto>>(p)).Returns(x);

            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            var result = await service.SearchProductsAsync("chai");

            Assert.NotNull(result);
            Assert.Equal(1, result.First().ProductId);
        }

        [Fact]
        public async Task GetProductsBySearch1()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(x => x.SearchProductsByNameAsync("")).ReturnsAsync(null as IEnumerable<Product>);
            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            await Assert.ThrowsAsync<BadRequestException>(() => service.SearchProductsAsync(""));
        }

        [Fact]
        public async Task UpdatePriceTest()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var prod = new Product
            {
                ProductId = 1,
                UnitPrice=500
            };
            mockRepo.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(prod);
            mockRepo.Setup(x => x.UpdateProductPriceAsync(1, 550)).Returns(Task.CompletedTask);
            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            await service.UpdatePriceAsync(1, 550);
            mockRepo.Verify(x =>x.UpdateProductPriceAsync(1, 550),Times.Once);
        }
        [Fact]
        public async Task UpdatePriceTest1()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            var prod = new Product
            {
                ProductId = 1,
                UnitPrice = 500
            };
            mockRepo.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(prod);
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdatePriceAsync(1, 0));
            Assert.Equal("Price should be greater than 0", ex.Message);
        }

        [Fact]
        public async Task AddProductTest()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var p = new Product
            {
                ProductName = "Coffee",
                UnitPrice = 20
            };
            var x = new ProductDetailsDto
            {
                ProductName = "Coffee",
                UnitPrice = 20
            };
            var u = new CreateProductDto
            {
                ProductName = "Coffee",
                UnitPrice = 20
            };
            mockMapper.Setup(x => x.Map<Product>(u)).Returns(p);
            mockRepo.Setup(x => x.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(p);
            //It.IsAny -- is a matcher from Moq it means that accept any object of type this.
            mockMapper.Setup(x => x.Map<ProductDetailsDto>(p)).Returns(x);

            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            var result = await service.AddProductAsync(u);
            Assert.NotNull(result);
            Assert.Equal("Coffee", result.ProductName);
            mockRepo.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once);
        }
        [Fact]
        public async Task AddProductTest1()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var service = new ProductService(mockRepo.Object, mockMapper.Object);
            await Assert.ThrowsAsync<BadRequestException>(() => service.AddProductAsync(new CreateProductDto()));
            
        }
    }
}