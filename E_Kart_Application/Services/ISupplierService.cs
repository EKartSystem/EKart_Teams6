using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductSummaryDto>> GetProductsBySupplierAsync(int supplierId); Task<IEnumerable<SupplierDto>> SearchByNameAsync(string name);
    Task<IEnumerable<SupplierDto>> GetByCountryAsync(string country);
    Task<IEnumerable<SupplierWithProductCountDto>> GetWithProductCountAsync();
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);
    Task<bool> UpdateContactAsync(int id, PatchSupplierContactDto dto);
    Task<bool> UpdateAddressAsync(int id, PatchSupplierAddressDto dto);
}
