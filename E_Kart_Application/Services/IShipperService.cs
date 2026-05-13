using E_Kart_Application.DTOs;

namespace E_Kart_Application.Services;

public interface IShipperService
{
    Task<IEnumerable<ShipperDto>> GetAllAsync();
    Task<ShipperDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId);
    Task<IEnumerable<ShipperDto>> SearchByNameAsync(string name);
    Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync();
    Task<ShipperDto> CreateAsync(ShipperRequestDto dto);
    Task<bool> UpdateAsync(int id, ShipperRequestDto dto);
    Task<bool> UpdateNameAsync(int id, PatchShipperNameDto dto);
    Task<bool> UpdatePhoneAsync(int id, PatchShipperPhoneDto dto);
    Task<bool> UpdateStatusAsync(int id, PatchShipperStatusDto dto);
}