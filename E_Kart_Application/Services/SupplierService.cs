using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _repository.GetByIdAsync(id);
        if (supplier == null) return null;
        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<IEnumerable<ProductSummaryDto>> GetProductsBySupplierAsync(int supplierId)
    {
        return await _repository.GetProductsBySupplierIdAsync(supplierId);
    }

    public async Task<IEnumerable<SupplierDto>> SearchByNameAsync(string name)
    {
        var suppliers = await _repository.SearchByNameAsync(name);
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<IEnumerable<SupplierDto>> GetByCountryAsync(string country)
    {
        var suppliers = await _repository.GetByCountryAsync(country);
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<IEnumerable<SupplierWithProductCountDto>> GetWithProductCountAsync()
    {
        return await _repository.GetWithProductCountAsync();
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        var supplier = _mapper.Map<Supplier>(dto);
        var created  = await _repository.CreateAsync(supplier);
        return _mapper.Map<SupplierDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;
        _mapper.Map(dto, existing);
        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> UpdateContactAsync(int id, PatchSupplierContactDto dto)
    {
        return await _repository.UpdateContactAsync(id, dto);
    }

    public async Task<bool> UpdateAddressAsync(int id, PatchSupplierAddressDto dto)
    {
        return await _repository.UpdateAddressAsync(id, dto);
    }
}
