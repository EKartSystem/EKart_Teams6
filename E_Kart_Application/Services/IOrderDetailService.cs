using E_Kart_Application.DTOs.OrderDetails;

namespace E_Kart_Application.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseDto>> GetAllAsync();
        Task<OrderDetailResponseDto?> GetByIdAsync(int orderId, int productId);
        Task<IEnumerable<OrderDetailResponseDto>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<OrderDetailResponseDto>> GetByOrderIdAsync(int orderId);
        Task<OrderDetailResponseDto> CreateAsync(CreateOrderDetailDto dto);
        Task<OrderDetailResponseDto?> UpdateAsync(int orderId, int productId, UpdateOrderDetailDto dto);
        Task DeleteAsync(int orderId, int productId);
    }
}