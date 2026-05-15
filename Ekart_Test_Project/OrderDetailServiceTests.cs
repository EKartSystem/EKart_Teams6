using AutoMapper;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Mappings;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Moq;

namespace Ekart_Test_Project.Services
{
    public class OrderDetailServiceTests
    {
        private readonly Mock<IOrderDetailRepository> _repositoryMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly IMapper _mapper;
        private readonly OrderDetailService _service;

        public OrderDetailServiceTests()
        {
            _repositoryMock = new Mock<IOrderDetailRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _orderRepoMock = new Mock<IOrderRepository>();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();
            _service = new OrderDetailService(
                _repositoryMock.Object,
                _productRepoMock.Object,
                _orderRepoMock.Object,
                _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrderDetail_WhenOrderDetailExists()
        {
            var orderDetail = new OrderDetail
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 5,
                UnitPrice = 100,
                Discount = 0
            };
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, 1))
                .ReturnsAsync(orderDetail);
            var result = await _service.GetByIdAsync(1, 1);
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(5, result.Quantity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrderDetails()
        {
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 5, UnitPrice = 100, Discount = 0 },
                new OrderDetail { OrderId = 1, ProductId = 2, Quantity = 3, UnitPrice = 200, Discount = 0 }
            };
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(orderDetails);
            var result = await _service.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrderDetailSuccessfully()
        {
            _productRepoMock.Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { ProductId = 1, ProductName = "Laptop", UnitPrice = 1000, UnitsInStock = 10 });
            _orderRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Order { OrderId = 1, CustomerId = "ALFKI", OrderDate = DateTime.Now });

            var dto = new CreateOrderDetailDto { OrderId = 1, ProductId = 1, Quantity = 2 };
            var orderDetail = new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 1000, Discount = 0 };

            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<OrderDetail>())).ReturnsAsync(orderDetail);

            var result = await _service.CreateAsync(dto);
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(2, result.Quantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrderDetailSuccessfully()
        {
            var existingOrderDetail = new OrderDetail
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 5,
                UnitPrice = 100,
                Discount = 0

            };
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, 1))
                .ReturnsAsync(existingOrderDetail);
            var updatedOrderDetail = new OrderDetail
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 10,
                UnitPrice = 100,
                Discount = 0
            };
            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<OrderDetail>()))
                .ReturnsAsync(updatedOrderDetail);
            var dto = new UpdateOrderDetailDto { Quantity = 10 };
            var result = await _service.UpdateAsync(1, 1, dto);
            Assert.NotNull(result);
            Assert.Equal(10, result.Quantity);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrderDetailSuccessfully()
        {
            _repositoryMock
                .Setup(r => r.DeleteAsync(1, 1))
                .ReturnsAsync(true);
            await _service.DeleteAsync(1, 1);
            _repositoryMock.Verify(r => r.DeleteAsync(1, 1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenOrderDetailDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999, 999))
                .ReturnsAsync((OrderDetail?)null);
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetByIdAsync(999, 999));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenQuantityIsZero()
        {
            var dto = new CreateOrderDetailDto
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 0
            };
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            _productRepoMock.Setup(r => r.GetProductByIdAsync(999)).ReturnsAsync((Product?)null);
            _orderRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Order { OrderId = 1, CustomerId = "ALFKI", OrderDate = DateTime.Now });

            var dto = new CreateOrderDetailDto { OrderId = 1, ProductId = 999, Quantity = 5 };
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
        {
            _productRepoMock.Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { ProductId = 1, ProductName = "Laptop", UnitPrice = 1000, UnitsInStock = 10 });
            _orderRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);

            var dto = new CreateOrderDetailDto { OrderId = 999, ProductId = 1, Quantity = 5 };
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowBadRequestException_WhenQuantityIsNegative()
        {
            var dto = new UpdateOrderDetailDto { Quantity = -1 };
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _service.UpdateAsync(1, 1, dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenOrderDetailDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999, 999))
                .ReturnsAsync((OrderDetail?)null);
            var dto = new UpdateOrderDetailDto { Quantity = 5 };
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateAsync(999, 999, dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenOrderDetailDoesNotExist()
        {
            _repositoryMock
                .Setup(r => r.DeleteAsync(999, 999))
                .ReturnsAsync(false);
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteAsync(999, 999));
        }
    }
}