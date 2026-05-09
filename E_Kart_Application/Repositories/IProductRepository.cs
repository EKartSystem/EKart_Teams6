using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllProductsInStock();

        Task<IEnumerable<ExpensiveProductDto>> GetExpensiveProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task UpdateAsync(Product product);
        Task UpdateProductPriceAsync(int id, decimal newPrice);
        Task UpdateProductStockAsync(int id, short units);
    }
}
