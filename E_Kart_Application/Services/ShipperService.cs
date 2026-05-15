using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.ShipperDto;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _repository;
    private readonly IMapper _mapper;

    public ShipperService(IShipperRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShipperDTO>> GetAllAsync()
    {
        var shippers = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ShipperDTO>>(shippers);
    }

    public async Task<ShipperDTO?> GetByIdAsync(int id)
    {
        var shipper = await _repository.GetByIdAsync(id);
        if (shipper == null) return null;
        return _mapper.Map<ShipperDTO>(shipper);
    }

    public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId)
    {
        return await _repository.GetOrdersByShipperIdAsync(shipperId);
    }

    public async Task<IEnumerable<ShipperDTO>> SearchByNameAsync(string name)
    {
        var shippers = await _repository.SearchByNameAsync(name);
        return _mapper.Map<IEnumerable<ShipperDTO>>(shippers);
    }

    public async Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync()
    {
        return await _repository.GetWithOrderCountAsync();
    }

    public async Task<ShipperDTO> CreateAsync(ShipperRequestDto dto)
    {
        var shipper = _mapper.Map<Shipper>(dto);
        var created = await _repository.CreateAsync(shipper);
        return _mapper.Map<ShipperDTO>(created);
    }

    public async Task<bool> UpdateAsync(int id, ShipperRequestDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;
        _mapper.Map(dto, existing);
        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> UpdateNameAsync(int id, PatchShipperNameDto dto)
    {
        return await _repository.UpdateNameAsync(id, dto.CompanyName);
    }

    public async Task<bool> UpdatePhoneAsync(int id, PatchShipperPhoneDto dto)
    {
        return await _repository.UpdatePhoneAsync(id, dto.Phone);
    }

}