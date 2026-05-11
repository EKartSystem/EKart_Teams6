using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    // The Service layer contains business logic.
    // It uses AutoMapper to convert between Models and DTOs.
    // It uses IShipperRepository to talk to the database.
    //
    // The Controller trusts this class to do the right thing
    // and just returns whatever the service gives back.
    public class ShipperService : IShipperService
    {
        private readonly IShipperRepository _repository;
        private readonly IMapper _mapper;

        public ShipperService(IShipperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        // GET all shippers — maps List<Shipper> → List<ShipperDto>
        public async Task<IEnumerable<ShipperDto>> GetAllAsync()
        {
            var shippers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
        }

        // GET one shipper by ID — returns null if not found (controller handles 404)
        public async Task<ShipperDto?> GetByIdAsync(int id)
        {
            var shipper = await _repository.GetByIdAsync(id);
            if (shipper == null) return null;
            return _mapper.Map<ShipperDto>(shipper);
        }

        // GET orders for a specific shipper (Admin analytics)
        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperAsync(int shipperId)
        {
            return await _repository.GetOrdersByShipperIdAsync(shipperId);
        }

        // GET search by name
        public async Task<IEnumerable<ShipperDto>> SearchByNameAsync(string name)
        {
            var shippers = await _repository.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
        }

        // GET with order count (the complex analytics query)
        public async Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync()
        {
            return await _repository.GetWithOrderCountAsync();
        }

        // POST — create new shipper
        // Maps CreateShipperDto → Shipper model → saves to DB → maps result back to ShipperDto
        public async Task<ShipperDto> CreateAsync(CreateShipperDto dto)
        {
            var shipper = _mapper.Map<Shipper>(dto);
            var created = await _repository.CreateAsync(shipper);
            return _mapper.Map<ShipperDto>(created);
        }

        // PUT — full update of existing shipper
        // Returns false if shipper not found (controller returns 404)
        public async Task<bool> UpdateAsync(int id, UpdateShipperDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            // Map the new values from DTO onto the existing model object
            _mapper.Map(dto, existing);
            return await _repository.UpdateAsync(existing);
        }

        // PATCH — update name only
        public async Task<bool> UpdateNameAsync(int id, PatchShipperNameDto dto)
        {
            return await _repository.UpdateNameAsync(id, dto.CompanyName);
        }

        // PATCH — update phone only
        public async Task<bool> UpdatePhoneAsync(int id, PatchShipperPhoneDto dto)
        {
            return await _repository.UpdatePhoneAsync(id, dto.Phone);
        }

        // PATCH — update active status only
        public async Task<bool> UpdateStatusAsync(int id, PatchShipperStatusDto dto)
        {
            return await _repository.UpdateStatusAsync(id, dto.IsActive);
        }
    }
}
