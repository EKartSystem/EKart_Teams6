using E_Kart_Application.DTOs.ProductsDTO;

namespace E_Kart_Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListingDto>> GetAllProductsAsync();
        Task<ProductDetailsDto?> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductListingDto>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<ExpensiveProductDto>> GetExpensiveProductsAsync();
        Task<IEnumerable<ProductListingDto>> SearchProductsAsync(string name);
        Task<IEnumerable<ProductListingDto>> GetInStockProductsAsync();
        Task<ProductDetailsDto> AddProductAsync(CreateProductDto createDto);
        Task UpdateProductAsync(int id, UpdateProductDto updateDto);
        Task UpdatePriceAsync(int id, decimal newPrice);
        Task UpdateStockAsync(int id, short units);
    }
}
