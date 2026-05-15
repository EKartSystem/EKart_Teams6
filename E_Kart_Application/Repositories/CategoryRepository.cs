using E_Kart_Application.DBContext;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EKARTContext _context;

        public CategoryRepository(EKARTContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(
                    x => x.CategoryId == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
        {
            return await _context.Products.Include(x => x.Category).Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string name)
        {
            return await _context.Categories.Where(x => x.CategoryName != null && x.CategoryName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync()
        {
            return await _context.Categories.Include(x => x.Products).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetEmptyCategoriesAsync()
        {
            return await _context.Categories.Where(x => !x.Products.Any()).ToListAsync();
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == category.CategoryId);

            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.CategoryName = category.CategoryName;

            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategoryNameAsync(int id, string? name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
            {
                return false;
            }

            category.CategoryName = name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategoryDescriptionAsync(int id, string? description)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(
                    x => x.CategoryId == id);

            if (category == null)
            {
                return false;
            }

            category.Description = description;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}