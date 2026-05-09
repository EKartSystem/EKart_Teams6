using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Models;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
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
    }
}
