using E_Kart_Application.Controllers;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace E_Kart_Application.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeService> _serviceMock;

        private readonly Mock<ILogger<EmployeesController>> _loggerMock;

        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _serviceMock = new Mock<IEmployeeService>();

            _loggerMock =
                new Mock<ILogger<EmployeesController>>();

            _controller = new EmployeesController(
                _serviceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetEmployees()
        {
            var employees = new List<EmployeeListingDto>
            {
                new EmployeeListingDto
                {
                    EmployeeId = 1,
                    FullName = "Mayank Sharma",
                    Title = "Developer",
                    IsManager = false,
                    TotalEmployeesUnderManager = 0
                }
            };

            _serviceMock
                .Setup(x => x.GetEmployeesAsync())
                .ReturnsAsync(employees);

            var result = await _controller
                .GetEmployees();

            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(
                employees,
                okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById()
        {
            var employee = new EmployeeDto
            {
                EmployeeId = 1,
                FirstName = "Mayank",
                LastName = "Sharma",
                Title = "Developer"
            };

            _serviceMock
                .Setup(x => x.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            var result = await _controller
                .GetEmployeeById(1);

            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(
                employee,
                okResult.Value);
        }

        [Fact]
        public async Task AddEmployee()
        {
            var dto = new CreateEmployeeDto
            {
                FirstName = "Mayank",
                LastName = "Sharma",
                Title = "Developer",
                Country = "India",
                HomePhone = "9876543210"
            };

            var employee = new EmployeeDto
            {
                EmployeeId = 1,
                FirstName = "Mayank",
                LastName = "Sharma",
                Title = "Developer"
            };

            _serviceMock
                .Setup(x => x.AddEmployeeAsync(dto))
                .ReturnsAsync(employee);

            var result = await _controller
                .AddEmployee(dto);

            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(
                employee,
                okResult.Value);
        }

        [Fact]
        public async Task UpdateEmployee()
        {
            var dto = new UpdateEmployeeDto
            {
                FirstName = "Updated",
                LastName = "Employee",
                Title = "Senior Developer"
            };

            _serviceMock
                .Setup(x =>
                    x.UpdateEmployeeAsync(1, dto))
                .ReturnsAsync(true);

            var result = await _controller
                .UpdateEmployee(1, dto);

            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.Equal(
                "Employee updated successfully",
                okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById2()
        {
            _serviceMock
                .Setup(x =>
                    x.GetEmployeeByIdAsync(99))
                .ReturnsAsync((EmployeeDto?)null);

            var result = await _controller
                .GetEmployeeById(99);

            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(
                    result);

            Assert.Equal(
                "Employee not found",
                notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateEmployee2()
        {
            var dto = new UpdateEmployeeDto
            {
                FirstName = "Invalid"
            };

            _serviceMock
                .Setup(x =>
                    x.UpdateEmployeeAsync(99, dto))
                .ReturnsAsync(false);

            var result = await _controller
                .UpdateEmployee(99, dto);

            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(
                    result);

            Assert.Equal(
                "Employee not found",
                notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateEmployeeTitle()
        {
            var dto = new UpdateEmployeeTitleDto
            {
                Title = "Manager"
            };

            _serviceMock
                .Setup(x =>
                    x.UpdateEmployeeTitleAsync(
                        99,
                        dto))
                .ReturnsAsync(false);

            var result = await _controller
                .UpdateEmployeeTitle(99, dto);

            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(
                    result);

            Assert.Equal(
                "Employee not found",
                notFoundResult.Value);
        }

        [Fact]
        public async Task AddTerritoryToEmployee()
        {
            _serviceMock
                .Setup(x =>
                    x.AddTerritoryToEmployeeAsync(
                        99,
                        "01581"))
                .ReturnsAsync(false);

            var result = await _controller
                .AddTerritoryToEmployee(
                    99,
                    "01581");

            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(
                    result);

            Assert.Equal(
                "Employee or territory not found",
                notFoundResult.Value);
        }
    }
}