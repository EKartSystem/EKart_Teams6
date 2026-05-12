using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EKARTContext _context;

        public ProductRepository(EKARTContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return await _context.Products.Include(x=>x.Category).Include(x=>x.Supplier)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
        }

        //public async Task<IEnumerable<Product>> GetAllProductsAsync()
        //{
        //    return await _context.Products.Include(x=>x.Category).ToListAsync();
        //}

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Products.CountAsync();
            var products = await _context.Products.Include(x => x.Category).OrderBy(x => x.ProductName) .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return (products, totalCount);
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products.Where(x => x.CategoryId == categoryId).Include(x=>x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsBySupplierAsync(int supplierId)
        {
            return await _context.Products.Where(x => x.SupplierId == supplierId).Include(x=>x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsInStock()
        {
            return await _context.Products.Where(x => x.UnitsInStock > 0).Include(x=>x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetExpensiveProductsAsync()
        {
            return await _context.Products.Include(x => x.Category).Include(x => x.Supplier) 
            .OrderByDescending(x => x.UnitPrice).Take(10).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.Include(x => x.Category).Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
        {
            return await _context.Products.Where(x => x.ProductName.Contains(name)).Include(x=>x.Category).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductPriceAsync(int id, decimal newPrice)
        {
            var x = await _context.Products.FindAsync(id);
            if (x != null)
            {
                x.UnitPrice = newPrice;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductStockAsync(int id, short units)
        {
            var x = await _context.Products.FindAsync(id);
            if (x!= null)
            {
                x.UnitsInStock = units;
                await _context.SaveChangesAsync();
            }
        }
    }
}
