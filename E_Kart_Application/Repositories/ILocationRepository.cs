using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface ILocationRepository
    {
        Task<List<Region>> GetAllRegionsAsync();
        Task<Region> GetRegionByIdAsync(int regionId);
        Task<List<Territory>> GetAllTerritoriesAsync();
        Task<List<Territory>> GetTerritoriesByRegionIdAsync(int regionId);
        Task<Territory> GetTerritoryByIdAsync(string territoryId);
        Task<Territory> CreateTerritoryAsync(Territory territory);
        Task UpdateTerritoryAsync(Territory territory);
    }
}
