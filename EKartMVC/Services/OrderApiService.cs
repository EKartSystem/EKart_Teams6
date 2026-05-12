using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.DTOs.Orders;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EKartMVC.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrderDto?> GetOrderAsync(int id, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient
                .GetFromJsonAsync<OrderDto>($"api/OrderApi/{id}");
        }

        public async Task<List<OrderDetailResponseDto>> GetDetailsAsync()
        {
            return await _httpClient
                .GetFromJsonAsync<List<OrderDetailResponseDto>>
                ("api/OrderDetails")
                ?? new List<OrderDetailResponseDto>();
        }

        public async Task<bool> CreateOrderAsync(CreateOrderDto dto)
        {
            var response = await _httpClient
                .PostAsJsonAsync("api/OrderApi", dto);
            return response.IsSuccessStatusCode;
        }
    }
}