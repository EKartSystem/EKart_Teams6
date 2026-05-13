using E_Kart_Application.DTOs.CategoryDto;

namespace E_Kart_Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<ResponseCategoryDto>> GetCategoriesAsync();

        Task<ResponseCategoryDto?> GetCategoryByIdAsync(int id);

        Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int id);

        Task<IEnumerable<ResponseCategoryDto>> SearchCategoriesAsync(string name);

        Task<IEnumerable<CategoryDto>>GetCategoriesWithProductCountAsync();

        Task<IEnumerable<ResponseCategoryDto>>GetEmptyCategoriesAsync();

        Task<ResponseCategoryDto>AddCategoryAsync(ResponseCategoryDto dto);

        Task<bool> UpdateCategoryAsync(int id,ResponseCategoryDto dto);

        Task<bool> UpdateCategoryNameAsync(int id, string? name);

        Task<bool> UpdateCategoryDescriptionAsync(int id, string? description);
    }
}