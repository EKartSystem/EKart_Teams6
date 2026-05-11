using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();

        Task<Employee?> GetEmployeeByIdAsync(int id);

        Task<IEnumerable<Employee>> GetManagersAsync();

        Task<IEnumerable<Employee>> GetEmployeesUnderManagerAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<bool> UpdateEmployeeAsync(int id, Employee employee);

        Task<bool> UpdateEmployeeTitleAsync(int id, string title);
        Task<IEnumerable<Territory>> Getemployeeterritory(int id);
        Task<bool> AddTerritoryToEmployeeAsync(int employeeId, string territoryId);


        }
}
