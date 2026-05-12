using System.Net.Http.Headers;
using E_Kart_Application.Common;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.ProductsDTO;
using EKartMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //public async Task<List<ProductListingDto>> GetDataAsync(string? token = null)
        //{
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }
        //    var response = await _httpClient.GetAsync("api/ProductsApi");
        //    if (response.IsSuccessStatusCode)
        //    { 
        //        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductListingDto>>>();
        //        return result?.Data ?? new List<ProductListingDto>();
        //    }
        //    return new List<ProductListingDto>();
        //}

        public async Task<PagedResponse<ProductListingDto>> GetPagedDataAsync(int pageNumber, int pageSize, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // API call with query parameters
            var response = await _httpClient.GetAsync($"api/ProductsApi/paged?pageNumber={pageNumber}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PagedResponse<ProductListingDto>>();
            }

            return new PagedResponse<ProductListingDto> { Data = new List<ProductListingDto>() };
        }
        public async Task<ProductDetailsDto> GetProductData(int id, string? token=null)
        {
            if(!String.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync($"api/ProductsApi/{id}");
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDetailsDto>>();
                return result?.Data;
            }
            return null;
        }

        public async Task<IEnumerable<ProductListingDto>> GetProductByName([FromQuery] string name, string? token =null)
        {
            if (!String.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync($"api/ProductsApi/search?name={Uri.EscapeDataString(name)}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductListingDto>>>();
                return result?.Data ?? Enumerable.Empty<ProductListingDto>();
            }
            return Enumerable.Empty<ProductListingDto>();
        }

        public async Task<List<ProductDetailsDto>> GetExpensiveProductAsync(string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("api/ProductsApi/expensiveProducts");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductDetailsDto>>>();
                return result?.Data ?? new List<ProductDetailsDto>();
            }
            return new List<ProductDetailsDto>();
        }

        public async Task<bool> CreateProductAsync(ProductDto dto, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/ProductsApi", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto dto, string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PutAsJsonAsync($"api/ProductsApi/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        // Categories fetch karne ke liye
        public async Task<List<CategoryDto>> GetCategoriesAsync(string? token = null)
        {
            // API endpoint: api/Categories ya jo bhi aapka path ho
            var response = await _httpClient.GetAsync("api/CategoriesApi");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                return result ?? new List<CategoryDto>();
            }
            return new List<CategoryDto>();
        }

        // Suppliers fetch karne ke liye
        public async Task<List<SupplierDto>> GetSuppliersAsync(string? token = null)
        {
            var response = await _httpClient.GetAsync("api/SuppliersApi");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<SupplierDto>>();
                return result ?? new List<SupplierDto>();
            }
            return new List<SupplierDto>();
        }

    }
}


