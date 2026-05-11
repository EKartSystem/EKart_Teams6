using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Models;

namespace E_Kart_Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();

        Task<CustomerDto?> GetCustomerByIdAsync(string id);

        Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(string id);

        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string name);

        Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country);

        Task<IEnumerable<CustomerDto>> GetTopCustomersAsync();

        Task<CustomerDto> RegisterCustomerAsync(RegisterCustomerDto dto);

        Task<CustomerDto?> LoginAsync(CustomerLogin dto);

        Task<bool> UpdateCustomerAsync(
            string id,
            UpdateCustomerDto dto);

        Task <bool> UpdateAddressAsync(
            string id,
             string address);

        Task<bool> UpdateContactAsync(
            string id,
            string contactName);
    }
}
