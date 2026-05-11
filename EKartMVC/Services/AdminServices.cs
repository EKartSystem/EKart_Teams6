using E_Kart_Application.DTOs.EmployeeDto;

namespace EKartMVC.Services
{
    public class AdminServices:IAdminService
    {
        private readonly HttpClient client;
        public AdminServices(HttpClient _client)
        {
            client = _client;

        }

        public async Task<IEnumerable<EmployeeListingDto>> GetEmployeeAll()
        {
            var employee = await client.GetFromJsonAsync<List<EmployeeListingDto>>("api/employees");
            return employee;

        }
    }
}
