using System.Net.Http.Headers;
using System.Net.Http.Json;
using EKartMVC.Models;
using System.Text.Json;

namespace EKartMVC.Services
{
    public class SupplierApiService
    {
        private readonly HttpClient _httpClient;

        public SupplierApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AddAuthToken(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<SupplierViewModel>> GetAllSuppliersAsync(string? token = null)
        {
            AddAuthToken(token);
            return await _httpClient.GetFromJsonAsync<List<SupplierViewModel>>("api/SuppliersApi")
                   ?? new List<SupplierViewModel>();
        }

        public async Task<SupplierViewModel?> GetSupplierByIdAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            return await _httpClient.GetFromJsonAsync<SupplierViewModel>($"api/SuppliersApi/{id}");
        }
        public async Task<List<dynamic>> GetSupplierProductsAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            var response = await _httpClient.GetAsync($"api/SuppliersApi/{id}/products");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<dynamic>>() ?? new List<dynamic>();
            }
            return new List<dynamic>();
        }

        public async Task<bool> CreateSupplierAsync(SupplierViewModel model, string? token = null)
        {
            AddAuthToken(token);
            var response = await _httpClient.PostAsJsonAsync("api/SuppliersApi", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSupplierAsync(int id, SupplierViewModel model, string? token = null)
        {
            AddAuthToken(token);
            var response = await _httpClient.PutAsJsonAsync($"api/SuppliersApi/{id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSupplierAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            var response = await _httpClient.DeleteAsync($"api/SuppliersApi/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<List<SupplierViewModel>> SearchSuppliersAsync(string name, string? token = null)
        {
            AddAuthToken(token);
            return await _httpClient.GetFromJsonAsync<List<SupplierViewModel>>($"api/SuppliersApi/search?name={Uri.EscapeDataString(name)}")
                   ?? new List<SupplierViewModel>();
        }
    }
}