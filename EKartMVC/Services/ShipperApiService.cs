using System.Net.Http.Headers;
using System.Net.Http.Json;
using EKartMVC.Models;

namespace EKartMVC.Services
{
    public class ShipperApiService
    {
        private readonly HttpClient _httpClient;

        public ShipperApiService(HttpClient httpClient)
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

        public async Task<List<ShipperViewModel>> GetAllShippersAsync(string? token = null)
        {
            AddAuthToken(token);
            return await _httpClient.GetFromJsonAsync<List<ShipperViewModel>>("api/ShippersApi")
                   ?? new List<ShipperViewModel>();
        }

        public async Task<ShipperViewModel?> GetShipperByIdAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            return await _httpClient.GetFromJsonAsync<ShipperViewModel>($"api/ShippersApi/{id}");
        }

        public async Task<(ShipperViewModel? Shipper, List<dynamic> Orders)> GetShipperOrdersAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            var shipper = await GetShipperByIdAsync(id, token);
            if (shipper == null) return (null, new List<dynamic>());

            AddAuthToken(token);
            var orders = await _httpClient.GetFromJsonAsync<List<dynamic>>($"api/ShippersApi/{id}/orders");
            return (shipper, orders ?? new List<dynamic>());
        }

        public async Task<bool> CreateShipperAsync(ShipperViewModel model, string? token = null)
        {
            AddAuthToken(token);
            var payload = new { companyName = model.CompanyName, phone = model.Phone };
            var response = await _httpClient.PostAsJsonAsync("api/ShippersApi", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateShipperAsync(int id, ShipperViewModel model, string? token = null)
        {
            AddAuthToken(token);
            var payload = new { companyName = model.CompanyName, phone = model.Phone };
            var response = await _httpClient.PutAsJsonAsync($"api/ShippersApi/{id}", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteShipperAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            var response = await _httpClient.DeleteAsync($"api/ShippersApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}