using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.ShipperDto;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Moq;
using Xunit;

namespace Ekart_Test_Project;

public class ShipperServiceTests
{
    private readonly Mock<IShipperRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ShipperService _service;

    public ShipperServiceTests()
    {
        _mockRepo = new Mock<IShipperRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new ShipperService(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllShippers()
    {
        var shippers = new List<Shipper>
        {
            new Shipper { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" },
            new Shipper { ShipperId = 2, CompanyName = "United Package",  Phone = "(503) 555-3199" }
        };
        var shipperDtos = new List<ShipperDTO>
        {
            new ShipperDTO { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" },
            new ShipperDTO { ShipperId = 2, CompanyName = "United Package",  Phone = "(503) 555-3199" }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(shippers);
        _mockMapper.Setup(m => m.Map<IEnumerable<ShipperDTO>>(shippers)).Returns(shipperDtos);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsShipperDto()
    {
        var shipper = new Shipper { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" };
        var shipperDto = new ShipperDTO { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(shipper);
        _mockMapper.Setup(m => m.Map<ShipperDTO>(shipper)).Returns(shipperDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.ShipperId);
        Assert.Equal("Speedy Express", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Shipper?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedShipperDto()
    {
        var requestDto = new ShipperRequestDto { CompanyName = "TestShipper", Phone = "1234567890" };
        var shipper = new Shipper { ShipperId = 4, CompanyName = "TestShipper", Phone = "1234567890" };
        var shipperDto = new ShipperDTO { ShipperId = 4, CompanyName = "TestShipper", Phone = "1234567890" };

        _mockMapper.Setup(m => m.Map<Shipper>(requestDto)).Returns(shipper);
        _mockRepo.Setup(r => r.CreateAsync(shipper)).ReturnsAsync(shipper);
        _mockMapper.Setup(m => m.Map<ShipperDTO>(shipper)).Returns(shipperDto);

        var result = await _service.CreateAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal(4, result.ShipperId);
        Assert.Equal("TestShipper", result.CompanyName);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_ReturnsTrue()
    {
        var requestDto = new ShipperRequestDto { CompanyName = "Updated Express", Phone = "9999999999" };
        var existing = new Shipper { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(requestDto, existing));
        _mockRepo.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

        var result = await _service.UpdateAsync(1, requestDto);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsFalse()
    {
        var requestDto = new ShipperRequestDto { CompanyName = "Ghost Shipper", Phone = "0000000000" };

        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Shipper?)null);

        var result = await _service.UpdateAsync(999, requestDto);

        Assert.False(result);
    }

    [Fact]
    public async Task SearchByNameAsync_ValidName_ReturnsMatchingShippers()
    {
        var shippers = new List<Shipper>
        {
            new Shipper { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" }
        };
        var shipperDtos = new List<ShipperDTO>
        {
            new ShipperDTO { ShipperId = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" }
        };

        _mockRepo.Setup(r => r.SearchByNameAsync("Speedy")).ReturnsAsync(shippers);
        _mockMapper.Setup(m => m.Map<IEnumerable<ShipperDTO>>(shippers)).Returns(shipperDtos);

        var result = await _service.SearchByNameAsync("Speedy");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Speedy Express", result.First().CompanyName);
    }

    [Fact]
    public async Task UpdateNameAsync_ExistingId_ReturnsTrue()
    {
        var patchDto = new PatchShipperNameDto { CompanyName = "New Express Name" };

        _mockRepo.Setup(r => r.UpdateNameAsync(1, "New Express Name")).ReturnsAsync(true);

        var result = await _service.UpdateNameAsync(1, patchDto);

        Assert.True(result);
    }
}