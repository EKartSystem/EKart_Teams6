using AutoMapper;
using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Mappings;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Ekart_Test_Project.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly EKARTContext _context;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            var options = new DbContextOptionsBuilder<EKARTContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w =>
                    w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _context = new EKARTContext(options);
            _service = new OrderService(
                _repositoryMock.Object,
                _mapper,
                _context);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            var order = new Order
            {
                OrderId = 1,
                CustomerId = "ALFKI"
            };
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(order);
            var result = await _service.GetOrderByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
            Assert.Equal("ALFKI", result.CustomerId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetOrderByIdAsync(1));
        }

        [Fact]
        public async Task GetRecentOrdersAsync_ShouldThrowBadRequestException_WhenCountIsInvalid()
        {
            await Assert.ThrowsAsync<BadRequestException>(() =>
             _service.GetRecentOrdersAsync(0));
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    CustomerId = "ALFKI"
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = "ANATR"
                }
            };
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(orders);
            var result = await _service.GetAllOrdersAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);
            var dto = new UpdateOrderDto
            {
                Freight = 100
            };
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateOrderAsync(1, dto));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowBadRequestException_WhenNoProductsExist()
        {
            var dto = new CreateOrderDto
            {
                CustomerId = "ALFKI",
                Products = new List<CreateOrderItemDto>()
            };
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _service.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowNotFoundException_WhenCustomerDoesNotExist()
        {
            _context.Products.Add(new Product
            {
                ProductId = 1,
                ProductName = "Laptop",
                UnitPrice = 1000,
                UnitsInStock = 10
            });
            await _context.SaveChangesAsync();
            var dto = new CreateOrderDto
            {
                CustomerId = "INVALID",
                Products = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrderSuccessfully()
        {
            _context.Customers.Add(new Customer
            {
                CustomerId = "ALFKI",
                CompanyName = "Test Company",
                Role = "Customer"
            });
            _context.Products.Add(new Product
            {
                ProductId = 1,
                ProductName = "Laptop",
                UnitPrice = 1000,
                UnitsInStock = 10
            });
            await _context.SaveChangesAsync();
            var dto = new CreateOrderDto
            {
                CustomerId = "ALFKI",
                Products = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };
            var result = await _service.CreateOrderAsync(dto);
            Assert.NotNull(result);
            Assert.Equal("ALFKI", result.CustomerId);
            var orderDetailsCount = _context.OrderDetails.Count();
            Assert.Equal(1, orderDetailsCount);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrderSuccessfully()
        {
            var order = new Order
            {
                OrderId = 1,
                CustomerId = "ALFKI",
                Freight = 50
            };
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(order);
            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order?)null);
            var dto = new UpdateOrderDto { Freight = 100 };
            var result = await _service.UpdateOrderAsync(1, dto);
            Assert.NotNull(result);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task GetOrdersByCustomerAsync_ShouldReturnOrdersForCustomer()
        {
            var customerId = "ALFKI";
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = customerId },
                new Order { OrderId = 2, CustomerId = customerId }
            };
            _repositoryMock
                .Setup(r => r.GetByCustomerAsync(customerId))
                .ReturnsAsync(orders);
            var result = await _service.GetOrdersByCustomerAsync(customerId);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowBadRequestException_WhenInsufficientStock()
        {
            _context.Customers.Add(new Customer
            {
                CustomerId = "ALFKI",
                CompanyName = "Test Company",
                Role = "Customer"
            });
            _context.Products.Add(new Product
            {
                ProductId = 1,
                ProductName = "Laptop",
                UnitPrice = 1000,
                UnitsInStock = 1
            });
            await _context.SaveChangesAsync();
            var dto = new CreateOrderDto
            {
                CustomerId = "ALFKI",
                Products = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 5
                    }
                }
            };
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _service.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            _context.Customers.Add(new Customer
            {
                CustomerId = "ALFKI",
                CompanyName = "Test Company",
                Role = "Customer"
            });
            await _context.SaveChangesAsync();
            var dto = new CreateOrderDto
            {
                CustomerId = "ALFKI",
                Products = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 999,
                        Quantity = 2
                    }
                }
            };
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task GetRecentOrdersAsync_ShouldReturnRecentOrders()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = "ALFKI" }
            };
            _repositoryMock
                .Setup(r => r.GetRecentAsync(5))
                .ReturnsAsync(orders);
            var result = await _service.GetRecentOrdersAsync(5);
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetRecentOrdersAsync_ShouldThrowBadRequestException_WhenCountIsNegative()
        {
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _service.GetRecentOrdersAsync(-1));
        }
    }
}