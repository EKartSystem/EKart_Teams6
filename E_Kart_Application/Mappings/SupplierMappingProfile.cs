using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierRequestDto, Supplier>();
    }
}