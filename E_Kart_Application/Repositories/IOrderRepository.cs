using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        //Task<IEnumerable<OrderDto>> GetByCustomerAsync(string customerId);
        Task<IEnumerable<Order>> GetByCustomerAsync(string customerId);
        Task<IEnumerable<Order>> GetRecentAsync(int count);
        Task<IEnumerable<Order>> GetByShipperAsync(int shipperId);
        Task<Order> CreateAsync(Order order);
        Task<Order?> UpdateAsync(Order order);
        Task SaveChangesAsync();

    }
}