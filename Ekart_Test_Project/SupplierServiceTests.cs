using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Moq;
using Xunit;

namespace Ekart_Test_Project;

public class SupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SupplierService _service;

    public SupplierServiceTests()
    {
        _mockRepo = new Mock<ISupplierRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new SupplierService(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSuppliers()
    {
        var suppliers = new List<Supplier>
        {
            new Supplier { SupplierId = 1, CompanyName = "Exotic Liquids",  Country = "UK"    },
            new Supplier { SupplierId = 2, CompanyName = "Tokyo Traders",   Country = "Japan" },
            new Supplier { SupplierId = 3, CompanyName = "Grandma Kelly's", Country = "USA"   }
        };
        var supplierDtos = new List<SupplierDto>
        {
            new SupplierDto { SupplierId = 1, CompanyName = "Exotic Liquids",  Country = "UK"    },
            new SupplierDto { SupplierId = 2, CompanyName = "Tokyo Traders",   Country = "Japan" },
            new SupplierDto { SupplierId = 3, CompanyName = "Grandma Kelly's", Country = "USA"   }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(suppliers);
        _mockMapper.Setup(m => m.Map<IEnumerable<SupplierDto>>(suppliers)).Returns(supplierDtos);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsSupplierDto()
    {
        var supplier = new Supplier { SupplierId = 1, CompanyName = "Exotic Liquids", Country = "UK" };
        var supplierDto = new SupplierDto { SupplierId = 1, CompanyName = "Exotic Liquids", Country = "UK" };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplier);
        _mockMapper.Setup(m => m.Map<SupplierDto>(supplier)).Returns(supplierDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.SupplierId);
        Assert.Equal("Exotic Liquids", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Supplier?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedSupplierDto()
    {
        var requestDto = new SupplierRequestDto
        {
            CompanyName = "TestSupplier",
            ContactName = "Test Person",
            Country = "India",
            Phone = "9876543210"
        };
        var supplier = new Supplier { SupplierId = 31, CompanyName = "TestSupplier", Country = "India" };
        var supplierDto = new SupplierDto { SupplierId = 31, CompanyName = "TestSupplier", Country = "India" };

        _mockMapper.Setup(m => m.Map<Supplier>(requestDto)).Returns(supplier);
        _mockRepo.Setup(r => r.CreateAsync(supplier)).ReturnsAsync(supplier);
        _mockMapper.Setup(m => m.Map<SupplierDto>(supplier)).Returns(supplierDto);

        var result = await _service.CreateAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal(31, result.SupplierId);
        Assert.Equal("TestSupplier", result.CompanyName);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_ReturnsTrue()
    {
        var requestDto = new SupplierRequestDto { CompanyName = "Updated Supplier", Country = "France" };
        var existing = new Supplier { SupplierId = 1, CompanyName = "Exotic Liquids", Country = "UK" };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(requestDto, existing));
        _mockRepo.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

        var result = await _service.UpdateAsync(1, requestDto);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsFalse()
    {
        var requestDto = new SupplierRequestDto { CompanyName = "Ghost Supplier", Country = "Nowhere" };

        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Supplier?)null);

        var result = await _service.UpdateAsync(999, requestDto);

        Assert.False(result);
    }

    [Fact]
    public async Task GetByCountryAsync_ValidCountry_ReturnsSuppliersFromCountry()
    {
        var suppliers = new List<Supplier>
        {
            new Supplier { SupplierId = 2,  CompanyName = "New Orleans Cajun Delights", Country = "USA" },
            new Supplier { SupplierId = 3,  CompanyName = "Grandma Kelly's Homestead",  Country = "USA" },
            new Supplier { SupplierId = 16, CompanyName = "Bigfoot Breweries",           Country = "USA" }
        };
        var supplierDtos = new List<SupplierDto>
        {
            new SupplierDto { SupplierId = 2,  CompanyName = "New Orleans Cajun Delights", Country = "USA" },
            new SupplierDto { SupplierId = 3,  CompanyName = "Grandma Kelly's Homestead",  Country = "USA" },
            new SupplierDto { SupplierId = 16, CompanyName = "Bigfoot Breweries",           Country = "USA" }
        };

        _mockRepo.Setup(r => r.GetByCountryAsync("USA")).ReturnsAsync(suppliers);
        _mockMapper.Setup(m => m.Map<IEnumerable<SupplierDto>>(suppliers)).Returns(supplierDtos);

        var result = await _service.GetByCountryAsync("USA");

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.All(result, s => Assert.Equal("USA", s.Country));
    }

    [Fact]
    public async Task UpdateContactAsync_ExistingId_ReturnsTrue()
    {
        var patchDto = new PatchSupplierContactDto
        {
            ContactName = "New Contact",
            ContactTitle = "Manager",
            Phone = "1112223333",
            Fax = "4445556666"
        };

        _mockRepo.Setup(r => r.UpdateContactAsync(1, patchDto)).ReturnsAsync(true);

        var result = await _service.UpdateContactAsync(1, patchDto);

        Assert.True(result);
    }
}