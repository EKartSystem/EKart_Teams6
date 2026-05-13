using AutoMapper;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;

namespace E_Kart_Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseCategoryDto>> GetCategoriesAsync()
        {
            var data = await _repo.GetCategoriesAsync();

            return _mapper.Map <IEnumerable<ResponseCategoryDto>>(data);
        }

        public async Task<ResponseCategoryDto?>GetCategoryByIdAsync(int id)
        {
            var data =await _repo.GetCategoryByIdAsync(id);
           return _mapper.Map<ResponseCategoryDto>(data);
        }

        public async Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int id)
        {
            var data = await _repo.GetProductsByCategoryAsync(id);

            return _mapper.Map<IEnumerable<ProductListingDto>>(data);
        }

        public async Task<IEnumerable<ResponseCategoryDto>> SearchCategoriesAsync(string name)
        {
            var data =await _repo.SearchCategoriesAsync(name);

            return _mapper.Map <IEnumerable<ResponseCategoryDto>>(data);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesWithProductCountAsync()
        {
            var data = await _repo.GetCategoriesWithProductCountAsync();

            return _mapper.Map <IEnumerable<CategoryDto>>(data);
        }

        public async Task<IEnumerable<ResponseCategoryDto>>GetEmptyCategoriesAsync()
        {
            var data = await _repo.GetEmptyCategoriesAsync();

            return _mapper.Map <IEnumerable<ResponseCategoryDto>>(data);
        }

        public async Task<ResponseCategoryDto>AddCategoryAsync(ResponseCategoryDto dto)
        {
            var category =
                _mapper.Map<Category>(dto);

            var data =
                await _repo.AddCategoryAsync(category);

            return _mapper.Map<ResponseCategoryDto>(data);
        }

        public async Task<bool>UpdateCategoryAsync(int id,ResponseCategoryDto dto)
        {
            var category =await _repo.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return false;
            }

            _mapper.Map(dto, category);

            return await _repo .UpdateCategoryAsync(category);
        }

        public async Task<bool>UpdateCategoryNameAsync( int id,string? name)
        {
            return await _repo .UpdateCategoryNameAsync( id, name);
        }

        public async Task<bool> UpdateCategoryDescriptionAsync(int id, string? description)
        {
            return await _repo.UpdateCategoryDescriptionAsync(id, description);
        }
    }
}