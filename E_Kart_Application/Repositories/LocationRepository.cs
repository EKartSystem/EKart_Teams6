
using E_Kart_Application.DBContext;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly EKARTContext _context;

        public LocationRepository(EKARTContext context)
        {
            _context = context;
        }
             
        public async Task<Territory> CreateTerritoryAsync(Territory territory)
        {
            await _context.Territories.AddAsync(territory);
            await _context.SaveChangesAsync();
            return territory;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return await _context.Regions.Include(x=>x.Territories).ToListAsync();
        }

        public async Task<List<Territory>> GetAllTerritoriesAsync()
        {
            return await _context.Territories.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(int regionId)
        {
            return await _context.Regions.Include(x=>x.Territories).FirstOrDefaultAsync(x=>x.RegionId==regionId);
        }

        public async Task<List<Territory>> GetTerritoriesByRegionIdAsync(int regionId)
        {
            return await _context.Territories.Include(x=>x.Region).Where(x => x.RegionId == regionId).ToListAsync();
        }

        public async Task<Territory> GetTerritoryByIdAsync(string territoryId)
        {
            return await _context.Territories.FindAsync(territoryId);
        }
        public async Task UpdateTerritoryAsync(Territory territory)
        {
            _context.Territories.Update(territory);
            await _context.SaveChangesAsync();
        }
    }
}
