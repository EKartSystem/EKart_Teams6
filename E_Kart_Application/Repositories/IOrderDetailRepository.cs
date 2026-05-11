using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();

        Task<OrderDetail?> GetByIdAsync(int orderId, int productId);

        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);

        Task<OrderDetail?> UpdateAsync(OrderDetail orderDetail);

        Task<bool> DeleteAsync(int orderId, int productId);
    }
}
