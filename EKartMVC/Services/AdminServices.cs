using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.DTOs.ProductsDTO;
using System.Net.Http.Headers;

namespace EKartMVC.Services
{
    public class AdminServices
    {
        private readonly HttpClient client;

        public AdminServices(HttpClient _client)
        {
            client = _client;
        }
        private void AddAuthToken(string? token)
        {
            client.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<IEnumerable<EmployeeListingDto>> GetEmployeeAll(string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<EmployeeListingDto>>("api/EmployeesApi") ?? new();
        }

        public async Task<ResponseEmployeeDto> GetEmployeebyId(int id, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<ResponseEmployeeDto>($"api/EmployeesApi/{id}");
        }

        public async Task<bool> Addemployee(ResponseEmployeeDto dto, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PostAsJsonAsync("api/EmployeesApi", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Updateemployee(int id, ResponseEmployeeDto dto, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PutAsJsonAsync($"api/EmployeesApi/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<EmployeeListingDto>> GetManagers(string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<IEnumerable<EmployeeListingDto>>("api/EmployeesApi/managers") ?? new List<EmployeeListingDto>();
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeeManagers(int id, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<IEnumerable<ResponseEmployeeDto>>($"api/EmployeesApi/reports/{id}") ?? new List<ResponseEmployeeDto>();
        }

        public async Task<bool> Updateemployeetitle(int id, string title, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PatchAsJsonAsync($"api/EmployeesApi/{id}/title", title);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<TerritoryDto>> GetTerritoriesAsync(int id, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<IEnumerable<TerritoryDto>>($"api/EmployeesApi/{id}/territories") ?? new List<TerritoryDto>();
        }

        public async Task<bool> Addterritories(int id, TerritoryDto dto, int territoryId, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PostAsJsonAsync($"api/EmployeesApi/{id}/territories/{territoryId}", dto);
            return response.IsSuccessStatusCode;
        }

        // --- CATEGORY METHODS ---

        public async Task<List<ResponseCategoryDto>?> GetCategories(string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/CategoriesApi");
        }

        public async Task<ResponseCategoryDto?> GetCategoryById(int id, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<ResponseCategoryDto>($"api/CategoriesApi/{id}");
        }

        public async Task<List<ProductListingDto>?> GetProductsByCategory(int id, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<ProductListingDto>>($"api/CategoriesApi/{id}/products");
        }

        public async Task<List<CategoryDto>?> GetCategoriesWithProductCount(string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<CategoryDto>>("api/CategoriesApi/with-product-count");
        }

        public async Task<List<ResponseCategoryDto>?> GetEmptyCategories(string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/CategoriesApi/empty");
        }

        public async Task<bool> AddCategory(ResponseCategoryDto dto, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PostAsJsonAsync("api/CategoriesApi", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(int id, ResponseCategoryDto dto, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PutAsJsonAsync($"api/CategoriesApi/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryName(int id, string name, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PatchAsJsonAsync($"api/CategoriesApi/{id}/name", name);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryDescription(int id, string description, string? token = null)
        {
            AddAuthToken(token);
            var response = await client.PatchAsJsonAsync($"api/CategoriesApi/{id}/description", description);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ResponseCategoryDto>?> SearchCategories(string name, string? token = null)
        {
            AddAuthToken(token);
            return await client.GetFromJsonAsync<List<ResponseCategoryDto>>($"api/CategoriesApi/search?name={name}");
        }
    }
}