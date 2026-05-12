using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.ProductsDTO;

namespace E_Kart_Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        Task<CategoryDto?> GetCategoryByIdAsync(int id);

        Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int id);

        Task<IEnumerable<CategoryDto>> SearchCategoriesAsync(string name);

        Task<IEnumerable<CategoryWithProductCountDto>>
            GetCategoriesWithProductCountAsync();

        Task<IEnumerable<CategoryDto>> GetEmptyCategoriesAsync();

        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto);

        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto);

        Task<bool> UpdateCategoryNameAsync(
            int id,
            UpdateCategoryNameDto dto);

        Task<bool> UpdateCategoryDescriptionAsync(
            int id,
            UpdateCategoryDescriptionDto dto);
    }
}
