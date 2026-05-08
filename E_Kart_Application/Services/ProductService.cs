using AutoMapper;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using Microsoft.CodeAnalysis;

namespace E_Kart_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _repo = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDetailsDto> AddProductAsync(CreateProductDto createDto)
        {
            var prod = _mapper.Map<Product>(createDto);
            await _repo.AddProductAsync(prod);
            return _mapper.Map<ProductDetailsDto>(prod);
        }

        public async Task<IEnumerable<ProductListingDto>> GetAllProductsAsync()
        {
            var prod = await _repo.GetAllProductsAsync();
            if (prod == null)
                throw new NotFoundException($"No Products Found");
            var products = _mapper.Map<IEnumerable<ProductListingDto>>(prod);
            return products;
        }

        public async Task<IEnumerable<ProductListingDto>> GetInStockProductsAsync()
        {
            var prod = await _repo.GetAllProductsInStock();
            if (prod == null)
                throw new NotFoundException($"No Products Found");
            var products = _mapper.Map<IEnumerable<ProductListingDto>>(prod);
            return products;
        }

        public async Task<ProductDetailsDto?> GetProductByIdAsync(int productId)
        {
            var prod = await _repo.GetProductByIdAsync(productId);
            if (prod == null)
                throw new NotFoundException($"No product with Id : {productId}");
            return _mapper.Map<ProductDetailsDto>(prod);
        }

        public async Task<IEnumerable<ProductListingDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var prod = await _repo.GetAllProductsByCategoryAsync(categoryId);
            if (prod == null)
                throw new NotFoundException($"No product with CategoryId : {categoryId}");
            var products =  _mapper.Map<IEnumerable<ProductListingDto>>(prod);
            return products;
        }

        public async Task<IEnumerable<ProductListingDto>> GetProductsBySupplierAsync(int supplierId)
        {
            var prod = await _repo.GetAllProductsBySupplierAsync(supplierId);
            if (prod == null)
                throw new NotFoundException($"No product with SupplierId : {supplierId}");
            var products = _mapper.Map<IEnumerable<ProductListingDto>>(prod);
            return products;
        }

        public async Task<IEnumerable<ProductListingDto>> SearchProductsAsync(string name)
        {
            var prod = await _repo.SearchProductsByNameAsync(name);
            if (prod == null)
                throw new NotFoundException($"No product with name : {name}");
            var products = _mapper.Map<IEnumerable<ProductListingDto>>(prod);
            return products;
        }

        public async Task UpdatePriceAsync(int id, decimal newPrice)
        {
            await _repo.UpdateProductPriceAsync(id, newPrice);
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            var prod = await _repo.GetProductByIdAsync(id);
            if (prod == null)
                throw new NotFoundException($"No product with Id : {id}");
            var x = _mapper.Map(updateDto, prod);
            await _repo.UpdateAsync(x);
        }

        public async Task UpdateStockAsync(int id, short units)
        {
            await _repo.UpdateProductStockAsync(id, units);
        }
    }
}
