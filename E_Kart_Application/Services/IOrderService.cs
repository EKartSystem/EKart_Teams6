using E_Kart_Application.DTOs.Orders;

namespace E_Kart_Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerId);
        Task<IEnumerable<OrderDto>> GetRecentOrdersAsync(int count);
        Task<IEnumerable<OrderDto>> GetOrdersByShipperAsync(int shipperId);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderDto dto);
        Task<OrderDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto);
        Task<OrderDto?> UpdateOrderAddressAsync(int id, UpdateOrderAddressDto dto);
        Task<OrderDto?> UpdateOrderShipperAsync(int id, UpdateOrderShipperDto dto);
    }
}