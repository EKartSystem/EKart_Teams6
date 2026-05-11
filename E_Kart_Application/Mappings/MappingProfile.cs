using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductListingDto>()
            .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.UnitsInStock > 0));

            CreateMap<Product, ProductDetailsDto>().ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.Category.CategoryName))

                .ForMember(dest => dest.CategoryDescription,opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.SupplierCompanyName,opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.SupplierCountry,opt => opt.MapFrom(src => src.Supplier.Country))
                .ForMember(dest => dest.IsInStock,opt => opt.MapFrom(src => src.UnitsInStock > 0));

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<TerritoryDto, Territory>().ReverseMap();
            CreateMap<RegionDto, Region>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ForMember(dest => dest.TotalOrders,opt => opt.MapFrom(src => src.Orders.Count)).ReverseMap();

            CreateMap<Customer, RegisterCustomerDto>().ForMember(dest => dest.Password,opt => opt.Ignore())
            .ForMember(dest => dest.Role,opt => opt.Ignore()).ReverseMap();

            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

            CreateMap<Customer, UpdateAddressDto>().ReverseMap();

            CreateMap<Customer, UpdateContactDto>().ReverseMap();

            CreateMap<Order, OrderDto>();
        }
    }
}
