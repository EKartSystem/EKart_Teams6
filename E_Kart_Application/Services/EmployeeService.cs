using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeListingDto>>GetEmployeesAsync()
        {
            var data = await _repository.GetEmployeesAsync();

            return _mapper.Map< IEnumerable<EmployeeListingDto>>(data);
        }

        public async Task<ResponseEmployeeDto?>GetEmployeeByIdAsync(int id)
        {
            var data = await _repository.GetEmployeeByIdAsync(id);

            if (data == null)
            {
                throw new NotFoundException(
                    "Employee not found");
            }

            return _mapper.Map<ResponseEmployeeDto>(data);
        }

        public async Task<IEnumerable<EmployeeListingDto>>GetManagersAsync()
        {
            var data = await _repository.GetManagersAsync();

            return _mapper.Map<IEnumerable<EmployeeListingDto>>(data);
        }

        public async Task<IEnumerable<ResponseEmployeeDto>>GetEmployeesUnderManagerAsync(int id)
        {
            var data = await _repository.GetEmployeesUnderManagerAsync(id);

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(data);
        }

        public async Task<ResponseEmployeeDto> AddEmployeeAsync(ResponseEmployeeDto dto)
        {
            if (dto.ReportsTo != null)
            {
                var manager = await _repository
                    .GetEmployeeByIdAsync(
                        dto.ReportsTo.Value);

                if (manager == null)
                {
                    throw new BadRequestException(
                        "Manager not found");
                }
            }

            var employee = _mapper.Map<Employee>(dto);

            var data = await _repository.AddEmployeeAsync(employee);

            return _mapper.Map<ResponseEmployeeDto>(data);
        }

        public async Task<bool>UpdateEmployeeAsync(int id, ResponseEmployeeDto dto)
        {
            var existingEmployee =await _repository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            if (dto.ReportsTo == id)
            {
                throw new BadRequestException("Employee cannot report to self");
            }

            if (dto.ReportsTo != null)
            {
                var manager = await _repository.GetEmployeeByIdAsync(dto.ReportsTo.Value);

                if (manager == null)
                {
                    throw new BadRequestException("Manager not found");
                }
            }

            var employee = _mapper.Map<Employee>(dto);

            return await _repository.UpdateEmployeeAsync(id,employee);
        }

       public async Task<bool> UpdateEmployeeTitleAsync(int id, string title)
{
    var employee = await _repository.GetEmployeeByIdAsync(id);

    if (employee == null)
    {
        return false;
    }

    employee.Title = title;

    await _repository.UpdateEmployeeAsync(id, employee);

    return true;
}

        public async Task<IEnumerable<TerritoryDto>> GetEmployeeTerritory(int id)
        {
            var employee = await _repository.Getemployeeterritory(id);

            return _mapper.Map<IEnumerable<TerritoryDto>>(employee);
        }

        public async Task<bool>AddTerritoryToEmployeeAsync(int employeeId,string territoryId)
        {
            var employee = await _repository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            var result = await _repository.AddTerritoryToEmployeeAsync(employeeId,territoryId);

            if (!result)
            {
                throw new BadRequestException("Territory not found");
            }

            return true;
        }
    }
}