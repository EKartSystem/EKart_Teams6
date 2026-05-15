using E_Kart_Application.DBContext;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;
using E_Kart_Application.DBContext;
using System.Net.Sockets;
namespace E_Kart_Application.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EKARTContext _context;

        public EmployeeRepository(EKARTContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees
                .Include(x => x.InverseReportsToNavigation)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(x => x.InverseReportsToNavigation)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
        }

        public async Task<IEnumerable<Employee>> GetManagersAsync()
        {
            return await _context.Employees
                .Include(x => x.InverseReportsToNavigation)
                .Where(x => x.InverseReportsToNavigation.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesUnderManagerAsync(int id)
        {
            return await _context.Employees
                .Where(x => x.ReportsTo == id)
                .ToListAsync();
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            var data = await _context.Employees
                .FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (data == null)
            {
                return false;
            }

            data.FirstName = employee.FirstName;
            data.LastName = employee.LastName;
            data.Title = employee.Title;
            data.City = employee.City;
            data.Country = employee.Country;
            data.HomePhone = employee.HomePhone;
            data.ReportsTo = employee.ReportsTo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEmployeeTitleAsync(int id, string title)
        {
            var data = await _context.Employees
                .FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (data == null)
            {
                return false;
            }

            data.Title = title;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Territory>> Getemployeeterritory(int id)
        {

            var employee = await _context.Employees.Include(x => x.Territories).FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return new List<Territory>();
            }
            return employee.Territories;

        }
        public async Task<bool> AddTerritoryToEmployeeAsync(int employeeId, string territoryId)
        {
            var employee = await _context.Employees.Include(x => x.Territories).FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            if (employee == null)
            {
                return false;
            }

            var territory = await _context.Territories.FirstOrDefaultAsync(x => x.TerritoryId == territoryId);

            if (territory == null)
            {
                return false;
            }

            employee.Territories.Add(territory);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}