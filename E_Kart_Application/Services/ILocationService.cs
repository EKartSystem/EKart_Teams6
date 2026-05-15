using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Models;

namespace E_Kart_Application.Services
{
    public interface ILocationService
    {
        Task<List<RegionDto>> GetAllRegionsAsync();
        Task<RegionDto> GetRegionByIdAsync(int regionId);
        Task<List<TerritoryDto>> GetAllTerritoriesAsync();
        Task<List<TerritoryDto>> GetTerritoriesByRegionIdAsync(int regionId);
        Task<TerritoryDto> GetTerritoryByIdAsync(string territoryId);
        Task<TerritoryDto> CreateTerritoryAsync(TerritoryDto territoryDto);
        Task UpdateTerritoryAsync(string Id, TerritoryDto territoryDto);
    }
}
