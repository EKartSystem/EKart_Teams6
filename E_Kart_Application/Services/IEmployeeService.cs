using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;

namespace E_Kart_Application.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeListingDto>> GetEmployeesAsync();

        Task<ResponseEmployeeDto?> GetEmployeeByIdAsync(int id);

        Task<IEnumerable<EmployeeListingDto>> GetManagersAsync();

        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesUnderManagerAsync(int id);

        Task<ResponseEmployeeDto> AddEmployeeAsync(ResponseEmployeeDto dto);

        Task<bool> UpdateEmployeeAsync(int id, ResponseEmployeeDto dto);

        Task<bool> UpdateEmployeeTitleAsync(int id,string title);
        Task<IEnumerable<TerritoryDto>> GetEmployeeTerritory(int id);
        Task<bool> AddTerritoryToEmployeeAsync(int employeeId, string territoryId);

    }
}