using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly EKARTContext _context;

    public SupplierRepository(EKARTContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers.FindAsync(id);
    }

    public async Task<IEnumerable<ProductSummaryDto>> GetProductsBySupplierIdAsync(int supplierId)
    {
        return await _context.Products
            .Where(p => p.SupplierId == supplierId)
            .Select(p => new ProductSummaryDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                Discontinued = p.Discontinued
            })
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Supplier>> SearchByNameAsync(string name)
    {
        return await _context.Suppliers
            .Where(s => s.CompanyName.ToLower().Contains(name.ToLower()))
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Supplier>> GetByCountryAsync(string country)
    {
        return await _context.Suppliers
            .Where(s => s.Country != null && s.Country.ToLower() == country.ToLower())
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierWithProductCountDto>> GetWithProductCountAsync()
    {
        return await _context.Suppliers
            .Select(s => new SupplierWithProductCountDto
            {
                SupplierId   = s.SupplierId,
                CompanyName  = s.CompanyName,
                Country      = s.Country,
                ContactName  = s.ContactName,
                Phone        = s.Phone,
                ProductCount = _context.Products.Count(p => p.SupplierId == s.SupplierId)
            })
            .OrderByDescending(x => x.ProductCount)
            .ToListAsync();
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<bool> UpdateAsync(Supplier supplier)
    {
        _context.Entry(supplier).State = EntityState.Modified;
        var rows = await _context.SaveChangesAsync();
        return rows > 0;
    }

    public async Task<bool> UpdateContactAsync(int id, PatchSupplierContactDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        supplier.ContactName  = dto.ContactName;
        supplier.ContactTitle = dto.ContactTitle ?? supplier.ContactTitle;
        supplier.Phone        = dto.Phone;
        supplier.Fax          = dto.Fax;

        var rows = await _context.SaveChangesAsync();
        return rows > 0;
    }

    public async Task<bool> UpdateAddressAsync(int id, PatchSupplierAddressDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        supplier.Address    = dto.Address;
        supplier.City       = dto.City;
        supplier.Region     = dto.Region;
        supplier.PostalCode = dto.PostalCode;
        supplier.Country    = dto.Country;

        var rows = await _context.SaveChangesAsync();
        return rows > 0;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Suppliers.AnyAsync(s => s.SupplierId == id);
    }
}
