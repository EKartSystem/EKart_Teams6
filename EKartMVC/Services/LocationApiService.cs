using System.Net.Http.Headers;
using E_Kart_Application.DTOs.LocationDTO;

namespace EKartMVC.Services
{
    public class LocationApiService
    {
        private readonly HttpClient _httpClient;
        public LocationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        public async Task<List<RegionDto>> GetAllRegionsAsync(string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("api/LocationApi/regions");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<RegionDto>>() ?? new List<RegionDto>();
            }
            return new List<RegionDto>();
        }

        public async Task<List<TerritoryDto>> GetTerritoriesByRegionAsync(int regionId, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"api/LocationApi/regions/{regionId}/territories");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TerritoryDto>>() ?? new List<TerritoryDto>();
            }
            return new List<TerritoryDto>();
        }

        public async Task<bool> CreateTerritoryAsync(TerritoryDto dto, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsJsonAsync("api/LocationApi/territories", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTerritoryAsync(string id, TerritoryDto dto, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PutAsJsonAsync($"api/LocationApi/territories/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<TerritoryDto?> GetTerritoryByIdAsync(string id, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync($"api/LocationApi/territories/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TerritoryDto>();
            }
            return null;
        }
    }
}