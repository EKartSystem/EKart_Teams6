using AutoMapper;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using NuGet.Protocol.Core.Types;

namespace E_Kart_Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>>GetCategoriesAsync()
        {
            var data = await _repository.GetCategoriesAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(data);
        }

        public async Task<CategoryDto?>GetCategoryByIdAsync(int id)
        {
            var data = await _repository.GetCategoryByIdAsync(id);

            if (data == null)
            {
                throw new NotFoundException("Category not found");
            }

            return _mapper.Map<CategoryDto>(data);
        }

        public async Task<IEnumerable<ProductListingDto>>GetProductsByCategoryAsync(int id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                throw new NotFoundException(
                    "Category not found");
            }

            var data = await _repository.GetProductsByCategoryAsync(id);

            return _mapper.Map<IEnumerable<ProductListingDto>>(data);
        }

        public async Task<IEnumerable<CategoryDto>>SearchCategoriesAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException("Category name is required");
            }

            var data = await _repository.SearchCategoriesAsync(name);

            return _mapper.Map<IEnumerable<CategoryDto>>(data);
        }

        public async Task<IEnumerable<CategoryWithProductCountDto>>GetCategoriesWithProductCountAsync()
        {
            var data = await _repository.GetCategoriesWithProductCountAsync();

            return _mapper.Map <IEnumerable<CategoryWithProductCountDto>>(data);
        }

        public async Task<IEnumerable<CategoryDto>> GetEmptyCategoriesAsync()
        {
            var data = await _repository.GetEmptyCategoriesAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(data);
        }

        public async Task<CategoryDto>AddCategoryAsync(CreateCategoryDto dto)
        {
            var categories = await _repository.SearchCategoriesAsync(dto.CategoryName!);

            if (categories.Any())
            {
                throw new BadRequestException("Category already exists");
            }

            var category = _mapper.Map<Category>(dto);

            var result = await _repository.AddCategoryAsync(category);

            return _mapper.Map<CategoryDto>(result);
        }

        public async Task<bool> UpdateCategoryAsync(int id,UpdateCategoryDto dto)
        {
            var existingCategory = await _repository.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                throw new NotFoundException("Category not found");
            }

            var category = _mapper.Map<Category>(dto);

            category.CategoryId = id;

            return await _repository.UpdateCategoryAsync(category);
        }

        public async Task<bool>UpdateCategoryNameAsync(int id,UpdateCategoryNameDto dto)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            return await _repository.UpdateCategoryNameAsync(id,dto.CategoryName!);
        }

        public async Task<bool>UpdateCategoryDescriptionAsync( int id, UpdateCategoryDescriptionDto dto)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            return await _repository.UpdateCategoryDescriptionAsync( id, dto.Description!);
        }
    }
}