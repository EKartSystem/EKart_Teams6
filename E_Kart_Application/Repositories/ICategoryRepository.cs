using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);

        Task<IEnumerable<Category>> SearchCategoriesAsync(string name);

        Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync();

        Task<IEnumerable<Category>> GetEmptyCategoriesAsync();

        Task<Category> AddCategoryAsync(Category category);

        Task<bool> UpdateCategoryAsync(Category category);

        Task<bool> UpdateCategoryNameAsync(int id, string? name);

        Task<bool> UpdateCategoryDescriptionAsync(int id, string? description);
    }
}