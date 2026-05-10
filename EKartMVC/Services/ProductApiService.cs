using System.Net.Http.Headers;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Common;

namespace EKartMVC.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductListingDto>> GetDataAsync(string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("api/ProductsApi");
            if (response.IsSuccessStatusCode)
            { 
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductListingDto>>>();
                return result?.Data ?? new List<ProductListingDto>();
            }
            return new List<ProductListingDto>();
        }
    }
}


