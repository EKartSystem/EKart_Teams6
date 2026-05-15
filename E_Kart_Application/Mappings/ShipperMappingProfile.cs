using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.ShipperDto;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings;

public class ShipperMappingProfile : Profile
{
    public ShipperMappingProfile()
    {
        CreateMap<Shipper, ShipperDTO>();
        CreateMap<ShipperRequestDto, Shipper>();
    }
}