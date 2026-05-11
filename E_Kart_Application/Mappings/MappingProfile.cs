using AutoMapper;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.Customersdto;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.DTOs.ProductsDTO;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, DTOs.ProductsDTO.ProductListingDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.UnitsInStock > 0))
                .ReverseMap();

            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.SupplierCompanyName, opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.SupplierCountry, opt => opt.MapFrom(src => src.Supplier.Country))
                .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.UnitsInStock > 0));

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryNameDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDescriptionDto>().ReverseMap();
            CreateMap<Category, CategoryWithProductCountDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));


            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeTitleDto>().ReverseMap();
            CreateMap<Employee, EmployeeListingDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.IsManager, opt => opt.MapFrom(src => src.InverseReportsToNavigation.Any()))
                .ForMember(dest => dest.TotalEmployeesUnderManager, opt => opt.MapFrom(src => src.InverseReportsToNavigation.Count))
                .ReverseMap();

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

            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<UpdateOrderAddressDto, Order>();
            CreateMap<UpdateOrderStatusDto, Order>();
            CreateMap<UpdateOrderShipperDto, Order>();

            CreateMap<OrderDetail, OrderDetailResponseDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
            CreateMap<UpdateOrderDetailDto, OrderDetail>();

            CreateMap<TerritoryDto, Territory>().ReverseMap();
            CreateMap<RegionDto, Region>().ReverseMap();
        }
    }
}