using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.DTOs.OrderDetails;
using EKartMVC.Models;
using EKartMVC.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace EKartMVC.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void Auth(string? token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public string GetCustomerIdFromToken(string? token)
        {
            if (string.IsNullOrEmpty(token)) return string.Empty;
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public async Task<bool> CreateOrderAsync(CreateOrderDto dto, string? token = null)
        {
            Auth(token);
            var response = await _httpClient.PostAsJsonAsync("api/OrderApi", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync(string? token = null)
        {
            Auth(token);
            return await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/OrderApi") ?? new();
        }

        public async Task<OrderDto?> GetOrderAsync(int id, string? token = null)
        {
            Auth(token);
            return await _httpClient.GetFromJsonAsync<OrderDto>($"api/OrderApi/{id}");
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerAsync(string customerId, string? token = null)
        {
            Auth(token);
            return await _httpClient.GetFromJsonAsync<List<OrderDto>>($"api/OrderApi/customer/{customerId}") ?? new();
        }

        public async Task<List<OrderDetailResponseDto>> GetOrderDetailsByOrderIdAsync(int orderId, string? token = null)
        {
            Auth(token);
            return await _httpClient.GetFromJsonAsync<List<OrderDetailResponseDto>>($"api/OrderDetails/order/{orderId}") ?? new();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, DateTime? shippedDate, string token)
        {
            Auth(token);
            var response = await _httpClient.PatchAsJsonAsync($"api/OrderApi/{orderId}/status", shippedDate);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, UpdateOrderDto dto, string token)
        {
            Auth(token);
            var response = await _httpClient.PutAsJsonAsync($"api/OrderApi/{orderId}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<PaginationViewModel<OrderListItemViewModel>> GetPagedOrdersAsync(int pageNumber, int pageSize, string token)
        {
            var allOrders = await GetAllOrdersAsync(token);
            int totalItems = allOrders.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var paged = allOrders.OrderByDescending(o => o.OrderId).Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var viewModels = new List<OrderListItemViewModel>();
            foreach (var order in paged)
            {
                var details = await GetOrderDetailsByOrderIdAsync(order.OrderId, token);
                viewModels.Add(new OrderListItemViewModel(order, details.Sum(d => d.Quantity * d.UnitPrice)));
            }

            return new PaginationViewModel<OrderListItemViewModel>(viewModels, pageNumber, pageSize, totalItems);
        }

        public async Task<CreateOrderDto?> BuildCheckoutDtoAsync(List<CartItem> cart, string token, CustomerApiService customerApiService)
        {
            var customerId = GetCustomerIdFromToken(token);
            if (string.IsNullOrEmpty(customerId)) return null;

            var customer = await customerApiService.GetProfileAsync(customerId, token);
            return new CreateOrderDto
            {
                CustomerId = customerId,
                ShipName = customer?.ContactName,
                ShipAddress = customer?.Address,
                ShipCity = customer?.City,
                ShipRegion = customer?.Region,
                ShipCountry = customer?.Country,
                Products = cart.Select(c => new CreateOrderItemDto
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList()
            };
        }

        public async Task<bool> UpdateFulfillmentAsync(int id, int? employeeId, int? shipVia, decimal? freight, string token)
        {
            var order = await GetOrderAsync(id, token);
            if (order == null) return false;

            var dto = new UpdateOrderDto
            {
                EmployeeId = employeeId,
                ShipVia = shipVia,
                Freight = freight,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry
            };

            return await UpdateOrderAsync(id, dto, token);
        }

        public async Task<List<OrderListItemViewModel>> GetCustomerOrderViewModelsAsync(string customerId, string token)
        {
            var orders = (await GetOrdersByCustomerAsync(customerId, token)).OrderByDescending(o => o.OrderId);
            var result = new List<OrderListItemViewModel>();
            foreach (var order in orders)
            {
                var details = await GetOrderDetailsByOrderIdAsync(order.OrderId, token);
                result.Add(new OrderListItemViewModel(order, details.Sum(od => od.Quantity * od.UnitPrice)));
            }
            return result;
        }
    }
}
