using E_Kart_Application.DTOs.EmployeeDto;

namespace EKartMVC.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<EmployeeListingDto>> GetEmployeeAll();
    }
}
