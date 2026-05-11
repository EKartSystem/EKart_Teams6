using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;

namespace E_Kart_Application.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeListingDto>> GetEmployeesAsync();

        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);

        Task<IEnumerable<EmployeeListingDto>> GetManagersAsync();

        Task<IEnumerable<EmployeeDto>> GetEmployeesUnderManagerAsync(int id);

        Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto dto);

        Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);

        Task<bool> UpdateEmployeeTitleAsync(int id, UpdateEmployeeTitleDto dto);
        Task<IEnumerable<TerritoryDto>> GetEmployeeTerritory(int id);
        Task<bool> AddTerritoryToEmployeeAsync(int employeeId, string territoryId);

    }
}