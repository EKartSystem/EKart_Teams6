using AutoMapper;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Moq;

namespace Ekart_Test_Project
{
    public class LocationTests
    {
        [Fact]
        public async Task CreateTerritoryTest()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var x = new Territory
            {
                TerritoryDescription = "New Territory"
            };
            var y = new TerritoryDto
            {
                TerritoryDescription = "New Territory"
            };
            mockmapper.Setup(m => m.Map<Territory>(y)).Returns(x);
            mockrepo.Setup(r => r.CreateTerritoryAsync(It.IsAny<Territory>())).ReturnsAsync(x);
            mockmapper.Setup(s => s.Map<TerritoryDto>(x)).Returns(y);

            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            var result = await service.CreateTerritoryAsync(y);
            Assert.NotNull(result);
            mockrepo.Verify(x => x.CreateTerritoryAsync(It.IsAny<Territory>()), Times.Once);
        }

        [Fact]
        public async Task CreateTerritoryTest1()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            await Assert.ThrowsAsync<BadRequestException>(() => service.CreateTerritoryAsync(new TerritoryDto()));
        }

        [Fact]
        public async Task GetAllRegionsTest()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var reg = new List<Region>
            {
                new Region()
                {
                    RegionDescription="New Region"
                }
            };
            var regDto = new List<RegionDto>
            {
                new RegionDto()
                {
                    RegionDescription="New Region"
                }
            };
            mockrepo.Setup(x => x.GetAllRegionsAsync()).ReturnsAsync(reg);
            mockmapper.Setup(x => x.Map<List<RegionDto>>(reg)).Returns(regDto);
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            var result = await service.GetAllRegionsAsync();

            Assert.NotNull(result);
            Assert.Equal("New Region", result.First().RegionDescription);
            mockrepo.Verify(x => x.GetAllRegionsAsync(), Times.Once);
            
        }

        [Fact]
        public async Task GetALlRegionsTest1()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            await Assert.ThrowsAnyAsync<NotFoundException>(() => service.GetAllRegionsAsync());
        }

        [Fact]
        public async Task GetRegionByIdTest()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var prod = new Region
            {
                RegionDescription = "Region By ID"
            };
            var reg = new RegionDto
            {
                RegionDescription = "Region By ID"
            };
            mockrepo.Setup(x => x.GetRegionByIdAsync(1)).ReturnsAsync(prod);
            mockmapper.Setup(x => x.Map<RegionDto>(prod)).Returns(reg);
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            var result = await service.GetRegionByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Region By ID", result.RegionDescription);
            mockrepo.Verify(x => x.GetRegionByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetRegionByIdTest1()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            await Assert.ThrowsAnyAsync<NotFoundException>(() => service.GetRegionByIdAsync(1));
        }
        [Fact]
        public async Task UpdateTerritroyTest()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var ter = new Territory
            {
                TerritoryId="abc",
                TerritoryDescription = "Updated Terrr"
            };
            var terDto = new TerritoryDto
            {
                TerritoryId="abc",
                TerritoryDescription = "Updated Terr"
            };
            mockrepo.Setup(x => x.GetTerritoryByIdAsync("abc")).ReturnsAsync(ter);
            mockrepo.Setup(x => x.UpdateTerritoryAsync(ter)).Returns(Task.CompletedTask);
            mockmapper.Setup(x => x.Map<Territory>(terDto)).Returns(ter);
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            await service.UpdateTerritoryAsync("abc", terDto);
            mockrepo.Verify(x => x.UpdateTerritoryAsync(It.IsAny<Territory>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTerritoryTest1()
        {
            var mockrepo = new Mock<ILocationRepository>();
            var mockmapper = new Mock<IMapper>();
            var terDto = new TerritoryDto
            {
                TerritoryId = "abc",
                TerritoryDescription = "Updated Terr"
            };
            var service = new LocationService(mockrepo.Object, mockmapper.Object);
            await Assert.ThrowsAnyAsync<NotFoundException>(() => service.UpdateTerritoryAsync("abc", terDto));
        }
    }
}
