using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;

using E_Kart_Application.Services;
using E_Kart_Application.Repositories;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Models;

namespace Ekart_Test_Project
{
    public class CustomerApiTest
    {
        private readonly Mock<ICustomerRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly CustomerService _service;

        public CustomerApiTest()
        {
            _repoMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new CustomerService(
                _repoMock.Object,
                _mapperMock.Object);
        }
            
        

        [Fact]
        public async Task LoginCustomerTest1()
        {
            // Arrange

            var x1 = new CustomerLogin
            {
                ContactName = "Maria",
                Password = "Password@123",
                Role = "Customer"
            };

            var y = new Customer 
            {
                CustomerId = "C001",
                ContactName = "Maria",
                Role = "Customer"
            };

            var z = new CustomerDto 
            {
                CustomerId = "C001",
                ContactName = "Maria",
                Role = "Customer"
            };
           
            _repoMock.Setup(x =>
                x.LoginAsync(
                    x1.ContactName,
                    x1.Password,
                     x1.Role))
                .ReturnsAsync(y);

            _mapperMock.Setup(x =>
                x.Map<CustomerDto>(y))
                .Returns(z);

            // Act

            var result =
                await _service.LoginAsync(x1);// service method call kar rha h jisme repo se data aayega aur mapper se dto me convert hoga    

            // Assert

            result.Should().NotBeNull();
            result!.CustomerId.Should().Be("C001");
        }

        [Fact]
        public async Task GetCustomerByIdTest2()
        {
            // Arrange

            var y = new Customer
            {
                CustomerId = "C001",
                ContactName = "Maria"
            };

            var z = new CustomerDto
            {
                CustomerId = "C001",
                ContactName = "Maria"
            };

            _repoMock.Setup(x =>
                x.GetCustomerByIdAsync("C001"))
                .ReturnsAsync(y);

            _mapperMock.Setup(x =>
                x.Map<CustomerDto>(y))
                .Returns(z);

            // Act

            var result =
                await _service.GetCustomerByIdAsync("C001");

            // Assert

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCustomersTest3()
        {
            // Arrange

            var y1 = new List<Customer>
            {
                new Customer
                {
                    CustomerId = "C001",
                    ContactName = "Maria"
                }
            };

            var z1 = new List<CustomerDto>
            {
                new CustomerDto
                {
                    CustomerId = "C001",
                    ContactName = "Maria"
                }
            };

            _repoMock.Setup(x =>
                x.GetCustomersAsync())
                .ReturnsAsync(y1);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<CustomerDto>>(y1))
                .Returns(z1);   
            // Act

            var result =
                await _service.GetCustomersAsync();

            // Assert

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task SearchCustomersTest4()
        {
            // Arrange

            var x1= new List<Customer>
            {
                new Customer
                {
                    CustomerId = "C001",
                    ContactName = "Maria"
                }
            };

            var y = new List<CustomerDto>
            {
                new CustomerDto
                {
                    CustomerId = "C001",
                    ContactName = "Maria"
                }
            };

            _repoMock.Setup(x =>
                x.SearchCustomersAsync("Maria"))
                .ReturnsAsync(x1);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<CustomerDto>>(x1))
                .Returns(y);    
            // Act

            var result =
                await _service.SearchCustomersAsync("Maria");

            // Assert

            result.Should().NotBeEmpty();
        }

        

        [Fact]
        public async Task LoginTest()
        {
            // Arrange

            var loginDto = new CustomerLogin
            {
                ContactName = "Wrong",
                Password = "Wrong",
                Role = "Customer"
            };

            _repoMock.Setup(x =>
                x.LoginAsync(
                    loginDto.ContactName,
                    loginDto.Password,
                    loginDto.Role))
                .ReturnsAsync((Customer?)null);

            // Act

            var result =
                await _service.LoginAsync(loginDto);

            // Assert

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomerByIdTest()
        {
            // Arrange

            _repoMock.Setup(x =>
                x.GetCustomerByIdAsync("INVALID"))
                .ReturnsAsync((Customer?)null);

            // Act

            var result =
                await _service.GetCustomerByIdAsync("INVALID");

            // Assert

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomers()
        {
            // Arrange

            var customers = new List<Customer>();
            var customerDtos = new List<CustomerDto>();

            _repoMock.Setup(x =>
                x.GetCustomersAsync())
                .ReturnsAsync(customers);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<CustomerDto>>(customers))
                .Returns(customerDtos);

            // Act

            var result =
                await _service.GetCustomersAsync();

            // Assert

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchCustomers()
        {
            // Arrange

            var customers = new List<Customer>();
            var customerDtos = new List<CustomerDto>();

            _repoMock.Setup(x =>
                x.SearchCustomersAsync("XYZ"))
                .ReturnsAsync(customers);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<CustomerDto>>(customers))
                .Returns(customerDtos);

            // Act

            var result =
                await _service.SearchCustomersAsync("XYZ");

            // Assert

            result.Should().BeEmpty();
        }
    }
}