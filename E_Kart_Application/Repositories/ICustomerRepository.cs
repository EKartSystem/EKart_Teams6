using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();

        Task<Customer?> GetCustomerByIdAsync(string id);

        Task<IEnumerable<Order>> GetCustomerOrdersAsync(string id);

        Task<IEnumerable<Customer>> SearchCustomersAsync(string name);

        Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country);

        Task<IEnumerable<Customer>> GetTopCustomersAsync();

        Task<Customer> RegisterCustomerAsync(Customer customer);

        Task<Customer?> LoginAsync(
            string id,
            string password,
            string role);

        Task<bool> UpdateCustomerAsync(
            string id,
            Customer customer);

        Task<bool> UpdateAddressAsync(
            string id,
            string address);

        Task<bool> UpdateContactAsync(
            string id,
            string contact );
    }
}