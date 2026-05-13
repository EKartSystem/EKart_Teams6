using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    
    public class ShipperService : IShipperService
    {
        private readonly IShipperRepository _repository;
        private readonly IMapper _mapper;

        public ShipperService(IShipperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<IEnumerable<ShipperDto>> GetAllAsync()
        {
            var shippers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
        }

        public async Task<ShipperDto?> GetByIdAsync(int id)
        {
            var shipper = await _repository.GetByIdAsync(id);
            if (shipper == null) return null;
            return _mapper.Map<ShipperDto>(shipper);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId)
        {
            return await _repository.GetOrdersByShipperIdAsync(shipperId);
        }

        public async Task<IEnumerable<ShipperDto>> SearchByNameAsync(string name)
        {
            var shippers = await _repository.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
        }

        public async Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync()
        {
            return await _repository.GetWithOrderCountAsync();
        }

        public async Task<ShipperDto> CreateAsync(CreateShipperDto dto)
        {
            var shipper = _mapper.Map<Shipper>(dto);
            var created = await _repository.CreateAsync(shipper);
            return _mapper.Map<ShipperDto>(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateShipperDto dto)
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

        public async Task<bool> UpdateStatusAsync(int id, PatchShipperStatusDto dto)
        {
            return await _repository.UpdateStatusAsync(id, dto.IsActive);
        }
    }
}
