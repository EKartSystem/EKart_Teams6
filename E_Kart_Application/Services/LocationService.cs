using AutoMapper;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _repository = locationRepository;
            _mapper = mapper;
        }
        public async Task<TerritoryDto> CreateTerritoryAsync(TerritoryDto territoryDto)
        {
            var x = _mapper.Map<Territory>(territoryDto);
            var t = await _repository.CreateTerritoryAsync(x);
            if (t == null)
                throw new BadRequestException("Unable to add Territory");
            return _mapper.Map<TerritoryDto>(t);
        }

        public async Task<List<RegionDto>> GetAllRegionsAsync()
        {
            
            var x = await _repository.GetAllRegionsAsync();
            if (x == null)
                throw new NotFoundException("No Regions found.");
            var reg = _mapper.Map<List<RegionDto>>(x);
            return reg;
        }

        public async Task<List<TerritoryDto>> GetAllTerritoriesAsync()
        {
            var x = await _repository.GetAllTerritoriesAsync();
            if (x == null)
                throw new NotFoundException("No Territories found.");
            var ter = _mapper.Map<List<TerritoryDto>>(x);
            return ter;
        }

        public async Task<RegionDto> GetRegionByIdAsync(int regionId)
        {
            var x = await _repository.GetRegionByIdAsync(regionId);
            if (x == null)
                throw new NotFoundException("No Region found.");
            var reg = _mapper.Map<RegionDto>(x);
            return reg;
        }

        public async Task<List<TerritoryDto>> GetTerritoriesByRegionIdAsync(int regionId)
        {
            var x = await _repository.GetTerritoriesByRegionIdAsync(regionId);
            if (x == null)
                throw new NotFoundException("No Region found.");
            var terByRegion = _mapper.Map<List<TerritoryDto>>(x);
            return terByRegion;
        }

        public async Task<TerritoryDto> GetTerritoryByIdAsync(string territoryId)
        {
            var x = await _repository.GetTerritoryByIdAsync(territoryId);
            if (x == null)
                throw new NotFoundException("No Territory found.");
            var ter = _mapper.Map<TerritoryDto>(x);
            return ter;
        }

        public async Task UpdateTerritoryAsync(string Id, TerritoryDto territoryDto)
        {
            var x = await _repository.GetTerritoryByIdAsync(Id);
            if (x == null)
                throw new NotFoundException($"No Territory with Id : {Id}");
            var updated = _mapper.Map(territoryDto, x);
            await _repository.UpdateTerritoryAsync(updated);
        }


    }
}
