using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
namespace E_Kart_Application.Services
{
    // Service layer interface.
    // The Service sits BETWEEN the Controller and the Repository.
    //
    // Controller → calls Service (business logic)
    // Service    → calls Repository (database)
    //
    // The Service works with DTOs (not raw models).
    // The Repository works with raw Models (not DTOs).
    public interface IShipperService
    {
        Task<IEnumerable<ShipperDto>> GetAllAsync();

        Task<ShipperDto?> GetByIdAsync(int id);

        Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId);
        Task<IEnumerable<ShipperDto>> SearchByNameAsync(string name);

        Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync();

        Task<ShipperDto> CreateAsync(CreateShipperDto dto);

        Task<bool> UpdateAsync(int id, UpdateShipperDto dto);

        Task<bool> UpdateNameAsync(int id, PatchShipperNameDto dto);

        Task<bool> UpdatePhoneAsync(int id, PatchShipperPhoneDto dto);

        //Task<bool> UpdateStatusAsync(int id, PatchShipperStatusDto dto);
    }
}
