using E_Kart_Application.DTOs.ProductsDTO;

namespace E_Kart_Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListingDto>> GetAllProductsAsync();
        Task<ProductDetailsDto?> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductListingDto>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<ProductDetailsDto>> GetExpensiveProductsAsync();
        Task<IEnumerable<ProductListingDto>> SearchProductsAsync(string name);
        Task<IEnumerable<ProductListingDto>> GetInStockProductsAsync();
        Task<ProductDetailsDto> AddProductAsync(ProductDto createDto);
        Task UpdateProductAsync(int id, ProductDto updateDto);
        Task UpdatePriceAsync(int id, decimal newPrice);
        Task UpdateStockAsync(int id, short units);
    }
}
