using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;

using E_Kart_Application.Services;
using E_Kart_Application.Repositories;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.Models;
using E_Kart_Application.Exceptions;

namespace Ekart_Test_Project
{
    public class EmployeeApiTest
    {
        private readonly Mock<IEmployeeRepository> _repoMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly EmployeeService _service;

        public EmployeeApiTest()
        {
            _repoMock =
                new Mock<IEmployeeRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new EmployeeService(
                    _repoMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetEmployees_Positive_Test1()
        {
            var employees =
                new List<Employee>
                {
                    new Employee
                    {
                        EmployeeId = 1,
                        FirstName = "Nancy"
                    }
                };

            var dtos =
                new List<EmployeeListingDto>
                {
                    new EmployeeListingDto
                    {
                        EmployeeId = 1,
                        FullName = "Nancy"
                    }
                };

            _repoMock.Setup(x =>
                x.GetEmployeesAsync())
                .ReturnsAsync(employees);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<EmployeeListingDto>>(employees))
                .Returns(dtos);

            var result =
                await _service.GetEmployeesAsync();

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetEmployeeById_Positive_Test2()
        {
            var employee =
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Andrew"
                };

            var dto =
                new ResponseEmployeeDto
                {
                    FirstName = "Andrew"
                };

            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            _mapperMock.Setup(x =>
                x.Map<ResponseEmployeeDto>(employee))
                .Returns(dto);

            var result =
                await _service.GetEmployeeByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddEmployee_Positive_Test3()
        {
            var dto =
                new ResponseEmployeeDto
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

            var employee =
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John"
                };

            _mapperMock.Setup(x =>
                x.Map<Employee>(dto))
                .Returns(employee);

            _repoMock.Setup(x =>
                x.AddEmployeeAsync(employee))
                .ReturnsAsync(employee);

            _mapperMock.Setup(x =>
                x.Map<ResponseEmployeeDto>(employee))
                .Returns(dto);

            var result =
                await _service.AddEmployeeAsync(dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateEmployee_Positive_Test4()
        {
            var dto =
                new ResponseEmployeeDto
                {
                    FirstName = "Updated"
                };

            var employee =
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Old"
                };

            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            _mapperMock.Setup(x =>
                x.Map<Employee>(dto))
                .Returns(employee);

            _repoMock.Setup(x =>
                x.UpdateEmployeeAsync(1, employee))
                .ReturnsAsync(true);

            var result =
                await _service.UpdateEmployeeAsync(
                    1,
                    dto);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetEmployeeById_Negative_Test5()
        {
            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(100))
                .ReturnsAsync((Employee?)null);

            Func<Task> action = async () =>
                await _service.GetEmployeeByIdAsync(100);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddEmployee_Negative_Test6()
        {
            var dto =
                new ResponseEmployeeDto
                {
                    FirstName = "John",
                    ReportsTo = 99
                };

            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(99))
                .ReturnsAsync((Employee?)null);

            Func<Task> action = async () =>
                await _service.AddEmployeeAsync(dto);

            await action.Should()
                .ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateEmployee_Negative_Test7()
        {
            var dto =
                new ResponseEmployeeDto
                {
                    FirstName = "Test"
                };

            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(99))
                .ReturnsAsync((Employee?)null);

            Func<Task> action = async () =>
                await _service.UpdateEmployeeAsync(
                    99,
                    dto);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateEmployeeTitle_Negative_Test8()
        {
            _repoMock.Setup(x =>
                x.GetEmployeeByIdAsync(99))
                .ReturnsAsync((Employee?)null);

            var result =
                await _service.UpdateEmployeeTitleAsync(
                    99,
                    "Manager");

            result.Should().BeFalse();
        }
    }
}