using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories;

public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task<IEnumerable<ProductSummaryDto>> GetProductsBySupplierIdAsync(int supplierId); Task<IEnumerable<Supplier>> SearchByNameAsync(string name);
    Task<IEnumerable<Supplier>> GetByCountryAsync(string country);
    Task<IEnumerable<SupplierWithProductCountDto>> GetWithProductCountAsync();
    Task<Supplier> CreateAsync(Supplier supplier);
    Task<bool> UpdateAsync(Supplier supplier);
    Task<bool> UpdateContactAsync(int id, PatchSupplierContactDto dto);
    Task<bool> UpdateAddressAsync(int id, PatchSupplierAddressDto dto);
    Task<bool> ExistsAsync(int id);
}