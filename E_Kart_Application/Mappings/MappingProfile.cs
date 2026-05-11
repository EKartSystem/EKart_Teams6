using AutoMapper;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Product Mappings ---
            CreateMap<Product, ProductListingDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.UnitsInStock > 0));

            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.SupplierCompanyName, opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.SupplierCountry, opt => opt.MapFrom(src => src.Supplier.Country))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.UnitsInStock > 0));

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            // --- Location Mappings ---
            CreateMap<TerritoryDto, Territory>().ReverseMap();
            CreateMap<RegionDto, Region>().ReverseMap();

            // --- Customer Mappings ---
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Orders.Count))
                .ReverseMap();

            CreateMap<Customer, RegisterCustomerDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateAddressDto>().ReverseMap();
            CreateMap<Customer, UpdateContactDto>().ReverseMap();

            // --- Order Mappings (Merged from feature/orders) ---
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<UpdateOrderAddressDto, Order>();
            CreateMap<UpdateOrderStatusDto, Order>();
            CreateMap<UpdateOrderShipperDto, Order>();

            // --- Order Detail Mappings ---
            CreateMap<OrderDetail, OrderDetailResponseDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.ProductName)
                );
            CreateMap<UpdateOrderDetailDto, OrderDetail>();
        }
    }
}