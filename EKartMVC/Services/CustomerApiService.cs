using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.DTOs.Orders;
using EKartMVC.Models;
using System.Net.Http.Headers;

public class CustomerApiService
{
    private readonly HttpClient _httpClient;

    public CustomerApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> LoginAsync(CustomerLogin dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CustomerApi/login", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result?.Token;
    }

    public async Task<bool> RegisterAsync(RegisterCustomerDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CustomerApi/register", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<CustomerDto?> GetProfileAsync(string id, string token)
    {
        SetBearerToken(token);
        return await _httpClient.GetFromJsonAsync<CustomerDto>($"/api/CustomerApi/{Uri.EscapeDataString(id)}");
    }

    public async Task<List<CustomerDto>?> GetCustomersAsync(string token)
    {
        SetBearerToken(token);
        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("/api/CustomerApi");
    }

    public async Task<bool> UpdateCustomerAsync(string id, UpdateCustomerDto dto, string token)
    {
        SetBearerToken(token);
        var response = await _httpClient.PutAsJsonAsync($"/api/CustomerApi/{Uri.EscapeDataString(id)}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<OrderDto>?> GetOrdersAsync(string id, string token)
    {
        SetBearerToken(token);
        var response = await _httpClient.GetAsync($"/api/CustomerApi/{Uri.EscapeDataString(id)}/orders");

        if (!response.IsSuccessStatusCode)
            return new List<OrderDto>();

        return await response.Content.ReadFromJsonAsync<List<OrderDto>>();
    }

    public async Task<List<CustomerDto>?> SearchAsync(string name, string token)
    {
        SetBearerToken(token);
        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>($"/api/CustomerApi/search?name={Uri.EscapeDataString(name)}");
    }

    public async Task<List<CustomerDto>?> GetTopAsync(string token)
    {
        SetBearerToken(token);
        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("/api/CustomerApi/top");
    }

    public async Task<bool> UpdateAddressAsync(string id, string address, string token)
    {
        SetBearerToken(token);
        var response = await _httpClient.PatchAsJsonAsync($"/api/CustomerApi/{Uri.EscapeDataString(id)}/address", address);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateContactAsync(string id, string contact, string token)
    {
        SetBearerToken(token);
        var response = await _httpClient.PatchAsJsonAsync($"/api/CustomerApi/{Uri.EscapeDataString(id)}/contact", contact);
        return response.IsSuccessStatusCode;
    }

    private void SetBearerToken(string? token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }
}