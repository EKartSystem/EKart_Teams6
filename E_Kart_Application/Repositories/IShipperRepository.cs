using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    
    public interface IShipperRepository
    {
        Task<IEnumerable<Shipper>> GetAllAsync();

        Task<Shipper?> GetByIdAsync(int id);

        Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperIdAsync(int shipperId);
        Task<IEnumerable<Shipper>> SearchByNameAsync(string name);

        Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync();

        Task<Shipper> CreateAsync(Shipper shipper);

        Task<bool> UpdateAsync(Shipper shipper);

        Task<bool> UpdateNameAsync(int id, string newName);

        Task<bool> UpdatePhoneAsync(int id, string? newPhone);

        Task<bool> UpdateStatusAsync(int id, bool isActive);

        Task<bool> ExistsAsync(int id);
    }
}
