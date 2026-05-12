using E_Kart_Application.DTOs.Customersdto;
using NuGet.Protocol.Plugins;

public class CustomerApiService
{
    private readonly HttpClient _httpClient;

    public CustomerApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public class LoginResponse
    {
        public string Token { get; set; }
    }

    // 🔹 LOGIN → POST /api/customer (returns JWT)
    public async Task<string?> LoginAsync(CustomerLogin dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CustomerApi/login", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result?.Token;
    }

    // 🔹 REGISTER → POST /api/customers
    public async Task<bool> RegisterAsync(RegisterCustomerDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CustomerApi/register", dto);
        return response.IsSuccessStatusCode;
    }

    // 🔹 GET PROFILE → GET /api/customers/{id}
    public async Task<CustomerDto?> GetProfileAsync(string id, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await _httpClient.GetFromJsonAsync<CustomerDto>($"/api/customers/{id}");
    }
}