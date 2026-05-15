using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.ShipperDto;

namespace E_Kart_Application.Services;

public interface IShipperService
{
    Task<IEnumerable<ShipperDTO>> GetAllAsync();
    Task<ShipperDTO?> GetByIdAsync(int id);
    Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId);
    Task<IEnumerable<ShipperDTO>> SearchByNameAsync(string name);
    Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync();
    Task<ShipperDTO> CreateAsync(ShipperRequestDto dto);
    Task<bool> UpdateAsync(int id, ShipperRequestDto dto);
    Task<bool> UpdateNameAsync(int id, PatchShipperNameDto dto);
    Task<bool> UpdatePhoneAsync(int id, PatchShipperPhoneDto dto);
}